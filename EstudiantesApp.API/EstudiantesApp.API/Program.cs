using EstudiantesApp.API.Middlewares;
using EstudiantesApp.API.SwaggerDocumentation;
using EstudiantesApp.Repositorio.Interfaces;
using EstudiantesApp.Repositorio.Repositorios;
using EstudiantesApp.Servicio.Interfaces;
using EstudiantesApp.Servicio.Mapeos;
using EstudiantesApp.Servicio.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

// Configuración del logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "EstudiantesApp.API")
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/estudiantesapp-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 15,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();


    builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
    // AutoMapper
    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<EstudianteMapper>();
        cfg.AddProfile<EstudianteMateriaMapper>();
        cfg.AddProfile<MateriaMapper>();
        cfg.AddProfile<ProfesorMapper>();
    });
    // Repositorios
    builder.Services.AddScoped<IEstudianteRepositorio, EstudianteRepositorio>();
    builder.Services.AddScoped<IEstudianteMateriaRepositorio, EstudianteMateriaRepositorio>();
    builder.Services.AddScoped<IProfesorRepositorio, ProfesorRepositorio>();
    builder.Services.AddScoped<IMateriaRepositorio, MateriaRepositorio>();

    // Servicios
    builder.Services.AddScoped<IEstudianteServicio, EstudianteServicio>();
    builder.Services.AddScoped<IMateriaServicio, MateriaServicio>();
    builder.Services.AddScoped<IProfesorServicio, ProfesorServicio>();
    builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();

    // Configuración de JWT
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ClockSkew = TimeSpan.Zero
        };
    });


    //
    var CORSSettings = builder.Configuration.GetSection("Cors");
    var URLsHabilitadas = CORSSettings.GetSection("AllowedOrigins").Get<string[]>();

    // Configuración de CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("EstudianteApp.Cliente",
            builder => builder.WithOrigins(URLsHabilitadas)
                             .AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowCredentials());
    });

    // Configuración de Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Estudiantes API", Version = "v1" });

        // Configuración para JWT en Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT con rol de Estudiante o Profesor",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
        c.DocumentFilter<FiltroRequerimientoRolesDocumentacion>();
    });
    
    // Configuracion de Autorizacion Acceso
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Estudiante", policy => policy.RequireRole("Estudiante"));
        options.AddPolicy("Profesor", policy => policy.RequireRole("Profesor"));

        // Política para endpoints que pueden usar ambos roles
        options.AddPolicy("UsuarioActivo", policy =>
            policy.RequireRole("Estudiante", "Profesor"));
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    // Middleware pipeline
    app.UseSerilogRequestLogging(); // Middleware para logging de requests
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseCors("EstudianteApp.Cliente");
    app.UseAuthentication();
    app.UseAuthorization();
    //MiddleWare Personalizado para obtener el Usuario Actual
    app.UseMiddleware<UsuarioActualMiddleware>();
    // Middleware de logging personalizado
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

        await next();

        logger.LogInformation($"Response: {context.Response.StatusCode}");
    });

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


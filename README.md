# 🎓 EstudiantesApp

Aplicación desarrollada con ASP.NET Core 8 (Web API) para el backend y React + Vite para el frontend. Permite la gestión de estudiantes, materias y profesores, con autenticación y separación por capas.

---

## 📁 Estructura del Proyecto

PruebaTecnica_Mectronics_DilanRondon/

├── EstudiantesApp.API # Backend ASP.NET Core 8 (Web API)

├── EstudiantesApp.Cliente # Frontend React + Vite

├── EstudiantesApp.Repositorio # Capa de acceso a datos

├── EstudiantesApp.Servicios # Capa de lógica de negocio

├── EstudiantesApp.BD # Scripts de base de datos y procedimientos almacenados

└── README.md # Este archivo


---

## 🚀 Requisitos

### General
- Git
- SQL Server (para scripts)
- Editor: Visual Studio 2022+ y/o VS Code

### Backend (.NET Core)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Frontend (React)
- [Node.js 18+](https://nodejs.org/)
- [npm](https://www.npmjs.com/) o [Yarn](https://yarnpkg.com/)

---

## ⚙️ Instalación y ejecución

### 1. Clona el repositorio

```bash
git clone [URL Repositorio]
cd EstudiantesApp
```
Modifica el Nombre de la carpeta Base por el nombre de EstudiantesApp

### 2. Configura la base de datos
Abre SQL Server Management Studio.

Ejecuta los scripts en la carpeta EstudiantesApp.BD para crear la base de datos, tablas y procedimientos almacenados.


### 3. Ejecutar BackEnd

```
cd EstudiantesApp.API
dotnet restore
```

Para el proyecto Estudiantes.Pruebas, se debe pasar manualmente los archivos DLL de los proyectos API, Repositorio y Servicio a la carpeta bin/debug/net8.0 para poder ejecutarlo, estos archivos se encuentran en la carpeta bin de cada proyecto.

```
dotnet build
dotnet run
```

Modificar la cadena de conexion del archivo appsettings.json, por la base de datos local del equipo o instancia SQL Server. 

### 4. Ejecutar FrontEnd
```
cd EstudiantesApp.Cliente
npm install
npm run dev
```




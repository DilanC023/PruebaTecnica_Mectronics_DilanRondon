IF OBJECT_ID('sp_TEstudiantes', 'P') IS NOT NULL
	DROP PROC sp_TEstudiantes
GO

CREATE PROCEDURE sp_TEstudiantes
    @EstudianteId INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL,
    @PasswordHash NVARCHAR(255) = NULL,
    @Activo BIT = NULL,
    @Opcion VARCHAR(2) -- A - Adicionar
					   -- C - Consultar
					   -- M - Modificar/Actualizar
					   -- E - Eliminar/Deshabilitar
WITH RECOMPILE AS
BEGIN
    SET NOCOUNT ON
    
    -- Adicionar: Insertar nuevo estudiante
    IF @Opcion = 'A'
    BEGIN
        BEGIN TRY
            INSERT INTO TEstudiantes (Nombre, Email, PasswordHash)
            VALUES (@Nombre, @Email, @PasswordHash)
            
            SELECT SCOPE_IDENTITY() AS EstudianteId, 'Estudiante creado exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS EstudianteId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
    -- Consultar: Obtener estudiante(s)
    ELSE IF @Opcion = 'C'
    BEGIN
        IF @EstudianteId IS NULL
            -- Leer todos los estudiantes activos
            SELECT EstudianteId, Nombre, Email,PasswordHash, FechaRegistro, Activo 
            FROM TEstudiantes 
            WHERE Activo = 1
        ELSE
            -- Leer un estudiante específico
            SELECT EstudianteId, Nombre, Email,PasswordHash, FechaRegistro, Activo 
            FROM TEstudiantes 
            WHERE EstudianteId = @EstudianteId AND Activo = 1
    END
    ELSE IF @Opcion = 'CE'
    BEGIN
        SELECT 
            EstudianteId, 
            Nombre, 
            Email, 
            PasswordHash, 
            FechaRegistro, 
            Activo
        FROM TEstudiantes
        WHERE Email = @Email AND Activo = 1
    END
    -- Modificar: Actualizar estudiante
    ELSE IF @Opcion = 'M'
    BEGIN
        BEGIN TRY
            UPDATE TEstudiantes
            SET Nombre = ISNULL(@Nombre, Nombre),
                Email = ISNULL(@Email, Email),
                PasswordHash = ISNULL(@PasswordHash, PasswordHash)
            WHERE EstudianteId = @EstudianteId
            
            SELECT @EstudianteId AS EstudianteId, 'Estudiante actualizado exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS EstudianteId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
    -- Eliminar: Deshabilitar estudiante (borrado lógico)
    ELSE IF @Opcion = 'E'
    BEGIN
        UPDATE TEstudiantes
        SET Activo = 0
        WHERE EstudianteId = @EstudianteId
        
        SELECT @EstudianteId AS EstudianteId, 'Estudiante desactivado exitosamente' AS Mensaje
    END
    ELSE
    BEGIN
        SELECT -1 AS EstudianteId, 'Operación no válida' AS Mensaje
    END
END
GO
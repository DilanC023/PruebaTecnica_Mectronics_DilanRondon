IF OBJECT_ID('sp_TProfesores', 'P') IS NOT NULL
	DROP PROC sp_TProfesores
GO

CREATE PROCEDURE sp_TProfesores
    @ProfesorId INT = NULL,
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
    
    -- Adicionar: Insertar Nuevo Profesor
    IF @Opcion = 'A'
    BEGIN
        BEGIN TRY
            INSERT INTO TProfesores (Nombre, Email, PasswordHash)
            VALUES (@Nombre, @Email, @PasswordHash)
            
            SELECT SCOPE_IDENTITY() AS ProfesorId, 'Profesor creado exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS ProfesorId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
    -- Consultar: Obtener Profesor(es)
    ELSE IF @Opcion = 'C'
    BEGIN
        IF @ProfesorId IS NULL
            SELECT *,
			ProfesorId, 
			Nombre, 
            Email, 
            PasswordHash, 
            Activo
            FROM TProfesores
            WHERE Activo = 1
        ELSE
           SELECT 
			ProfesorId, 
			Nombre, 
            Email, 
            PasswordHash, 
            Activo
            FROM TProfesores
            WHERE ProfesorId = @ProfesorId AND Activo = 1
    END
    -- Consultar: Obtener Profesor(es)
    ELSE IF @Opcion = 'CE'
    BEGIN
        SELECT 
            ProfesorId, 
            Nombre, 
            Email, 
            PasswordHash, 
            Activo
        FROM TProfesores
        WHERE Email = @Email AND Activo = 1
    END
    -- Modificar: Actualizar Profesor
    ELSE IF @Opcion = 'M'
    BEGIN
        BEGIN TRY
            UPDATE TProfesores
            SET Nombre = ISNULL(@Nombre, Nombre)
            WHERE ProfesorId = @ProfesorId
            
            SELECT @ProfesorId AS ProfesorId, 'Profesor actualizado exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS ProfesorId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
    -- Eliminar: Deshabilitar Profesor (borrado lógico)
    ELSE IF @Opcion = 'E'
    BEGIN
        UPDATE TProfesores
        SET Activo = 0
        WHERE ProfesorId = @ProfesorId
        
        SELECT @ProfesorId AS ProfesorId, 'Profesor deshabilitar exitosamente' AS Mensaje
    END
    ELSE
    BEGIN
        SELECT -1 AS ProfesorId, 'Operación no válida' AS Mensaje
    END
END
GO
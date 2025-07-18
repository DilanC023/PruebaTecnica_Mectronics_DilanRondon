IF OBJECT_ID('sp_TMaterias', 'P') IS NOT NULL
	DROP PROC sp_TMaterias
GO

CREATE PROCEDURE sp_TMaterias
    @MateriaId INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @Creditos INT = NULL,
    @ProfesorId INT = NULL,
    @Activo BIT = NULL,
    @Opcion VARCHAR(2) -- A - Adicionar
					   -- C - Consultar
					   -- M - Modificar/Actualizar
					   -- E - Eliminar/Deshabilitar
WITH RECOMPILE AS
BEGIN
    SET NOCOUNT ON
    
     -- Adicionar: Insertar nueva materia
    IF @Opcion = 'A'
    BEGIN
        BEGIN TRY
            INSERT INTO TMaterias (Nombre, Creditos, ProfesorId)
            VALUES (@Nombre, ISNULL(@Creditos, 3), @ProfesorId)
            
            SELECT SCOPE_IDENTITY() AS MateriaId, 'Materia creada exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS MateriaId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
      -- Consultar: Obtener Materia(s)
    ELSE IF @Opcion = 'C'
    BEGIN
        IF @MateriaId IS NULL
            SELECT m.MateriaId, m.Nombre, m.Creditos, m.ProfesorId, p.Nombre AS ProfesorNombre,m.Activo
            FROM TMaterias m
            JOIN TProfesores p ON m.ProfesorId = p.ProfesorId
            WHERE m.Activo = 1
        ELSE
            SELECT m.MateriaId, m.Nombre, m.Creditos, m.ProfesorId, p.Nombre AS ProfesorNombre,m.Activo
            FROM TMaterias m
            JOIN TProfesores p ON m.ProfesorId = p.ProfesorId
            WHERE m.MateriaId = @MateriaId AND m.Activo = 1
    END
     -- Consultaar: Obtener Materias por Profesor
    ELSE IF @Opcion = 'CP'
    BEGIN
        SELECT 
            MateriaId, 
            Nombre, 
            Creditos, 
            ProfesorId,
            Activo
        FROM TMaterias
        WHERE ProfesorId = @ProfesorId AND Activo = 1;
    END
    -- Modificar: Actualizar Materia
    ELSE IF @Opcion = 'M'
    BEGIN
        BEGIN TRY
            UPDATE TMaterias
            SET Nombre = ISNULL(@Nombre, Nombre),
                Creditos = ISNULL(@Creditos, Creditos),
                ProfesorId = ISNULL(@ProfesorId, ProfesorId)
            WHERE MateriaId = @MateriaId
            
            SELECT @MateriaId AS MateriaId, 'Materia actualizada exitosamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT -1 AS MateriaId, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
  -- Eliminar: Deshabilitar Materia (borrado lógico)
    ELSE IF @Opcion = 'E'
    BEGIN
        UPDATE TMaterias
        SET Activo = 0
        WHERE MateriaId = @MateriaId
        
        SELECT @MateriaId AS MateriaId, 'Materia deshabilitada exitosamente' AS Mensaje
    END
    ELSE
    BEGIN
        SELECT -1 AS MateriaId, 'Operación no válida' AS Mensaje
    END
END
GO
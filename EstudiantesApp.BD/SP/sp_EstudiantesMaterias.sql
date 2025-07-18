IF OBJECT_ID('sp_TEstudiantesMaterias', 'P') IS NOT NULL
	DROP PROC sp_TEstudiantesMaterias
GO

CREATE PROCEDURE sp_TEstudiantesMaterias
    @EstudianteId INT = NULL,
    @MateriaId INT = NULL,
    @ValidarRestricciones BIT = 1, -- Parámetro para habilitar/deshabilitar validaciones
    @Opcion VARCHAR(10)-- A - Adicionar
					   -- C - Consultar
					   -- M - Modificar/Actualizar
					   -- E - Eliminar/Deshabilitar
WITH RECOMPILE AS
BEGIN
    SET NOCOUNT ON
    
    -- Adicionar: Inscribir materia
    IF @Opcion = 'A'
    BEGIN
        BEGIN TRY
            -- Validaciones (si están habilitadas)
            IF @ValidarRestricciones = 1
            BEGIN
                -- Validar que no tenga más de 3 materias
                IF (SELECT COUNT(*) FROM TEstudiantesMaterias WHERE EstudianteId = @EstudianteId) >= 3
                BEGIN
                    RAISERROR('El estudiante ya tiene el máximo de 3 materias inscritas', 16, 1)
                    RETURN
                END
                
                -- Validar que no esté ya inscrito en esta materia
                IF EXISTS (SELECT 1 FROM TEstudiantesMaterias WHERE EstudianteId = @EstudianteId AND MateriaId = @MateriaId)
                BEGIN
                    RAISERROR('El estudiante ya está inscrito en esta materia', 16, 1)
                    RETURN
                END
                
                -- Validar que no tenga más de una materia con el mismo profesor
                DECLARE @ProfesorId INT
                SELECT @ProfesorId = ProfesorId FROM TMaterias WHERE MateriaId = @MateriaId
                
                IF EXISTS (
                    SELECT 1 
                    FROM TEstudiantesMaterias em
                    JOIN TMaterias m ON em.MateriaId = m.MateriaId
                    WHERE em.EstudianteId = @EstudianteId AND m.ProfesorId = @ProfesorId
                )
                BEGIN
                    RAISERROR('No puedes tener más de una materia con el mismo profesor', 16, 1)
                    RETURN
                END
            END
            
            -- Inscribir la materia
            INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
            VALUES (@EstudianteId, @MateriaId)
            
            SELECT 1 AS Resultado, 'Materia inscrita correctamente' AS Mensaje
        END TRY
        BEGIN CATCH
            SELECT 0 AS Resultado, ERROR_MESSAGE() AS Mensaje
        END CATCH
    END
    
    -- Consultar: Obtener materias del estudiante
    ELSE IF @Opcion = 'C'
    BEGIN
        SELECT m.MateriaId, m.Nombre, m.Creditos, p.ProfesorId, p.Nombre AS ProfesorNombre
        FROM TEstudiantesMaterias em
        JOIN TMaterias m ON em.MateriaId = m.MateriaId
        JOIN TProfesores p ON m.ProfesorId = p.ProfesorId
        WHERE em.EstudianteId = @EstudianteId
        AND m.Activo = 1;
    END
    
    -- Eliminar: Eliminar inscripción
    ELSE IF @Opcion = 'E'
    BEGIN
        DELETE FROM TEstudiantesMaterias
        WHERE EstudianteId = @EstudianteId AND MateriaId = @MateriaId;
        
        SELECT @@ROWCOUNT AS FilasAfectadas;
    END
    
    -- CC: Obtener compañeros de clase
    ELSE IF @Opcion = 'CC'
    BEGIN
        SELECT e.EstudianteId, e.Nombre
        FROM TEstudiantesMaterias em
        JOIN TEstudiantes e ON em.EstudianteId = e.EstudianteId
        WHERE em.MateriaId = @MateriaId
        AND e.Activo = 1;
    END
    ELSE
    BEGIN
        SELECT -1 AS Resultado, 'Operación no válida' AS Mensaje
    END
END
GO
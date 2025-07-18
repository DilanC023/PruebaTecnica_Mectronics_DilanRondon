IF DB_ID('RegistroEstudiantes') IS NULL
	CREATE DATABASE RegistroEstudiantes
GO

USE RegistroEstudiantes
GO

-- Tabla Profesores
IF OBJECT_ID('TProfesores', 'U') IS NULL
BEGIN
	CREATE TABLE TProfesores (
		ProfesorId INT PRIMARY KEY IDENTITY(1,1),
		Nombre NVARCHAR(100) NOT NULL,
		Email NVARCHAR(100) NOT NULL UNIQUE,
		PasswordHash NVARCHAR(255) NOT NULL,
		FechaRegistro DATETIME DEFAULT GETDATE(),
		Activo BIT DEFAULT 1
	)
END
GO
-- Tabla Materias
IF OBJECT_ID('TMaterias', 'U') IS NULL
BEGIN
CREATE TABLE TMaterias (
    MateriaId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Creditos INT NOT NULL DEFAULT 3,
    ProfesorId INT NOT NULL,
    Activo BIT DEFAULT 1,
    CONSTRAINT FK_Materias_Profesores FOREIGN KEY (ProfesorId) REFERENCES TProfesores(ProfesorId)
)
END
GO
-- Tabla Estudiantes
IF OBJECT_ID('TEstudiantes', 'U') IS NULL
BEGIN
CREATE TABLE TEstudiantes (
    EstudianteId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
END
GO
-- Tabla de relación Estudiantes-Materias
IF OBJECT_ID('TEstudiantesMaterias', 'U') IS NULL
BEGIN
CREATE TABLE TEstudiantesMaterias (
    EstudianteId INT NOT NULL,
    MateriaId INT NOT NULL,
    FechaInscripcion DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_EstudiantesMaterias PRIMARY KEY (EstudianteId, MateriaId),
    CONSTRAINT FK_EstudiantesMaterias_Estudiantes FOREIGN KEY (EstudianteId) REFERENCES TEstudiantes(EstudianteId),
    CONSTRAINT FK_EstudiantesMaterias_Materias FOREIGN KEY (MateriaId) REFERENCES TMaterias(MateriaId)
)
END
GO

-- Insertar 5 profesores
INSERT INTO TProfesores (Nombre, Email, PasswordHash)
VALUES 
('Profesor García', 'garcia@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'clave123'), 2)),
('Profesor Martínez', 'martinez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'clave123'), 2)),
('Profesor López', 'lopez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'clave123'), 2)),
('Profesor Rodríguez', 'rodriguez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'clave123'), 2)),
('Profesor Sánchez', 'sanchez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'clave123'), 2))
GO

-- Insertar 10 materias, asignando 2 materias al mismo profesor (el primero, Profesor García)
INSERT INTO TMaterias (Nombre, Creditos, ProfesorId)
VALUES
('Matemáticas Avanzadas', 4, 1),          -- Profesor García
('Física Cuántica', 4, 1),                -- Profesor García (segunda materia)
('Literatura Universal', 3, 2),           -- Profesor Martínez
('Historia Contemporánea', 3, 3),         -- Profesor López
('Biología Molecular', 4, 4),             -- Profesor Rodríguez
('Química Orgánica', 4, 5),              -- Profesor Sánchez
('Programación Avanzada', 3, 2),          -- Profesor Martínez
('Derecho Internacional', 3, 3),          -- Profesor López
('Economía Global', 3, 4),                -- Profesor Rodríguez
('Arte Digital', 3, 5)                    -- Profesor Sánchez
GO
-- Insertar estudiantes
INSERT INTO TEstudiantes (Nombre, Email, PasswordHash)
VALUES
('Juan Pérez', 'juan.perez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'estudiante1'), 2)),
('María González', 'maria.gonzalez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'estudiante2'), 2)),
('Carlos Sánchez', 'carlos.sanchez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'estudiante3'), 2)),
('Ana Rodríguez', 'ana.rodriguez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'estudiante4'), 2)),
('Luis Martínez', 'luis.martinez@escuela.com', CONVERT(NVARCHAR(255), HASHBYTES('SHA2_256', 'estudiante5'), 2))
GO

-- Asignar materias a estudiantes en TEstudiantesMaterias
-- Estudiante 1 (Juan Pérez) toma 3 materias
INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
VALUES
(1, 1), -- Matemáticas Avanzadas
(1, 3), -- Literatura Universal
(1, 6)  -- Química Orgánica

-- Estudiante 2 (María González) toma 4 materias
INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
VALUES
(2, 2), -- Física Cuántica
(2, 4), -- Historia Contemporánea
(2, 7), -- Programación Avanzada
(2, 10) -- Arte Digital

-- Estudiante 3 (Carlos Sánchez) toma 2 materias
INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
VALUES
(3, 1), -- Matemáticas Avanzadas
(3, 5)  -- Biología Molecular

-- Estudiante 4 (Ana Rodríguez) toma 3 materias
INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
VALUES
(4, 3), -- Literatura Universal
(4, 6), -- Química Orgánica
(4, 9)  -- Economía Global

-- Estudiante 5 (Luis Martínez) toma 4 materias
INSERT INTO TEstudiantesMaterias (EstudianteId, MateriaId)
VALUES
(5, 2), -- Física Cuántica
(5, 4), -- Historia Contemporánea
(5, 7), -- Programación Avanzada
(5, 8)  -- Derecho Internacional
GO

-- Verificar los datos insertados
SELECT p.ProfesorId, p.Nombre AS Profesor, COUNT(m.MateriaId) AS MateriasImpartidas
FROM TProfesores p
LEFT JOIN TMaterias m ON p.ProfesorId = m.ProfesorId
GROUP BY p.ProfesorId, p.Nombre
ORDER BY p.ProfesorId
GO

SELECT m.MateriaId, m.Nombre AS Materia, m.Creditos, p.Nombre AS Profesor
FROM TMaterias m
JOIN TProfesores p ON m.ProfesorId = p.ProfesorId
ORDER BY m.MateriaId
GO

SELECT 
    e.EstudianteId,
    e.Nombre AS Estudiante,
    m.MateriaId,
    m.Nombre AS Materia,
    p.Nombre AS Profesor,
    em.FechaInscripcion
FROM TEstudiantes e
JOIN TEstudiantesMaterias em ON e.EstudianteId = em.EstudianteId
JOIN TMaterias m ON em.MateriaId = m.MateriaId
JOIN TProfesores p ON m.ProfesorId = p.ProfesorId
ORDER BY e.EstudianteId, m.MateriaId
GO
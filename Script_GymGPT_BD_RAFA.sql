-- Eliminar la base de datos si existe y crearla de nuevo
DROP DATABASE IF EXISTS SoftwareGPT_NewDB;
CREATE DATABASE SoftwareGPT_NewDB;
USE SoftwareGPT_NewDB;

-- Desactivar chequeos de clave foránea para borrado de tablas
SET FOREIGN_KEY_CHECKS = 0;
SET SQL_SAFE_UPDATES = 0;

-- Borrar tablas si existen
DROP TABLE IF EXISTS 
  Miembro,
  Miembro_Trabajador,
  Registro_Asistencias,
  Trabajador,
  Pago,
  Usuario_Trabajador,
  Tipo_Entreno,
  Miembro_VIP,
  Trabajador_tipo_Entreno,
  Membresia,
  Super_Admin_Pass,
  Clases_Personalizadas;

-- Re-activar chequeos de claves foráneas y actualizaciones seguras
SET FOREIGN_KEY_CHECKS = 1;
SET SQL_SAFE_UPDATES = 1;

-- Creación de tablas
CREATE TABLE Super_Admin_Pass (
  id_SAdmin INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
  HashPass NVARCHAR(255),
  Salt NVARCHAR(128)
);
CREATE TABLE Miembro (
  id_miembro INT AUTO_INCREMENT PRIMARY KEY,
  nombre NVARCHAR(30),
  apellido_p NVARCHAR(20),
  apellido_m NVARCHAR(20),
  ine CHAR(13) UNIQUE,
  telefono VARCHAR(15),
  correo_e NVARCHAR(40),
  activo BOOL,
  matricula CHAR(9) UNIQUE,
  id_membresia INT
);

create table Miembro_Trabajador
(
	id_miembro_trabajador int auto_increment primary key,
    id_trabajador int, #Debe de ser un entrenador
    #FOREIGN KEY (id_trabajador) REFERENCES Trabajador(id_trabajador),
    id_tipo_entreno int,
    #FOREIGN KEY (id_tipo_entreno) REFERENCES Tipo_Entreno(id_tipo_entreno),
    id_miembro int
    #FOREIGN KEY (id_miembro) REFERENCES Miembro(id_miembro)
);

CREATE TABLE Registro_Asistencias (
  id_registro_asistencias INT AUTO_INCREMENT PRIMARY KEY,
  id_miembro INT,
  fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE Pago (
  id_pago INT AUTO_INCREMENT PRIMARY KEY,
  id_miembro INT,
  cantidad DECIMAL(10,2),
  fecha_corte DATE,
  fecha_limite DATE,
  adeudo BOOL
);
CREATE TABLE Trabajador (
  id_trabajador INT AUTO_INCREMENT PRIMARY KEY,
  nombre NVARCHAR(30),
  apellido_p NVARCHAR(20),
  apellido_m NVARCHAR(20),
  ine CHAR(13) UNIQUE,
  telefono VARCHAR(15),
  correo_e NVARCHAR(40),
  matricula CHAR(9) UNIQUE
);
CREATE TABLE Usuario_Trabajador (
  id_usuario_trabajador INT AUTO_INCREMENT PRIMARY KEY,
  id_trabajador INT,
  isadmin BOOL, -- Diferencia entre admin (true) y entrenador (false)
  usuario NVARCHAR(50),
  HashPass NVARCHAR(255),
  Salt NVARCHAR(128)
);
CREATE TABLE Tipo_Entreno (
  id_tipo_entreno INT AUTO_INCREMENT PRIMARY KEY,
  nombre NVARCHAR(35)
);
CREATE TABLE Miembro_VIP (
  id_miembro_vip INT AUTO_INCREMENT PRIMARY KEY,
  id_miembro INT,
  id_trabajador_tipo_entreno INT
);
CREATE TABLE Trabajador_tipo_Entreno (
  id_trabajador_tipo_entreno INT AUTO_INCREMENT PRIMARY KEY,
  id_trabajador INT,
  id_tipo_entreno INT
);
CREATE TABLE Membresia (
  id_membresia INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nombre NVARCHAR(50),
  costo DECIMAL(10,00)
);
CREATE TABLE Clases_Personalizadas (
  id_clase_personalizada INT AUTO_INCREMENT PRIMARY KEY,
  id_entrenador INT,
  nombre_clase NVARCHAR(100),
  dias_semana NVARCHAR(60),
  hora_inicio TIME,
  hora_fin TIME,
  id_tipo_entreno INT
);

CREATE TABLE Horarios_Clase (
  id_horario INT AUTO_INCREMENT PRIMARY KEY,
  id_entrenador INT,
  id_tipo_entreno INT,
  dia VARCHAR(15),
  hora_inicio TIME,
  duracion TIME
);

-- Inserciones iniciales
INSERT INTO Super_Admin_Pass (HashPass, Salt) VALUES 
('$2a$10$u5Saedey0EGiHTGY3Wk5o.ueA1Br/pqwat.YUYXEqV18.Iq6KouZO', '$2a$10$DTvDLQt08Ms/ZXhscAkV1e');
insert into Miembro (nombre, apellido_p, apellido_m, ine, telefono, correo_e, activo, matricula, id_membresia) values 
('User','Cero','Default','33C45UJK9101Z','529994178512','userexample@mail', true,'101010109',1);
Insert into Pago (id_miembro, cantidad, fecha_corte, fecha_limite, adeudo) values 
(1, 420.00, '2023-11-10', '2023-11-16', true);
#Insertamos el admin que iniciara todo
Insert into Trabajador (nombre, apellido_p, apellido_m, ine, telefono, correo_e, matricula) values 
('Luis','Lopez','Flores','12C45UJK9101X','529995688572','userDD@mail','12CA67ZX9');
#Insertamos el entrenador base
Insert into Trabajador (nombre, apellido_p, apellido_m, ine, telefono, correo_e, matricula) values 
('Rafa','The Rock','Jhonson','B3C45UJK9109L','529993758589','therock@mail','12CA60PKM');
#El usuario del administrador
INSERT INTO Usuario_Trabajador (id_trabajador, isadmin, usuario, HashPass, Salt) values 
(1, true, 'Admin3322',  '$2a$10$u5Saedey0EGiHTGY3Wk5o.ueA1Br/pqwat.YUYXEqV18.Iq6KouZO', '$2a$10$DTvDLQt08Ms/ZXhscAkV1e');
#Entrenamiento por default
insert into Tipo_Entreno (nombre) values 
('Calistenia');
insert into Miembro_VIP (id_miembro, id_trabajador_tipo_entreno) values 
(1,1);
insert into Trabajador_tipo_Entreno (id_trabajador, id_tipo_entreno) values 
(2,1);
insert into Membresia (nombre, costo) values
('Entrenamiento basico', 420.00),
('Entrenamiento pro personal', 630.00);
INSERT INTO Clases_Personalizadas (id_entrenador, nombre_clase, dias_semana, hora_inicio, hora_fin, id_tipo_entreno)
VALUES (2, 'Yoga en la mañana', 'Lunes', '08:00:00', '09:30:00', 1);

INSERT INTO Horarios_Clase (id_entrenador, id_tipo_entreno, dia, hora_inicio, duracion)
VALUES (2, 1, 'Lunes', '08:00:00', '01:30:00');
-- Agregar de llaves foráneas
ALTER TABLE Miembro ADD FOREIGN KEY (id_membresia) REFERENCES Membresia(id_membresia);
ALTER TABLE Registro_Asistencias ADD FOREIGN KEY (id_miembro) REFERENCES Miembro(id_miembro);
ALTER TABLE Pago ADD FOREIGN KEY (id_miembro) REFERENCES Miembro(id_miembro);
ALTER TABLE Usuario_Trabajador ADD FOREIGN KEY (id_trabajador) REFERENCES Trabajador(id_trabajador);
ALTER TABLE Miembro_VIP ADD FOREIGN KEY (id_miembro) REFERENCES Miembro(id_miembro);
ALTER TABLE Trabajador_tipo_Entreno ADD FOREIGN KEY (id_trabajador) REFERENCES Trabajador(id_trabajador);
ALTER TABLE Trabajador_tipo_Entreno ADD FOREIGN KEY (id_tipo_entreno) REFERENCES Tipo_Entreno(id_tipo_entreno);
ALTER TABLE Clases_Personalizadas ADD FOREIGN KEY (id_entrenador) REFERENCES Trabajador(id_trabajador);
ALTER TABLE Clases_Personalizadas ADD FOREIGN KEY (id_tipo_entreno) REFERENCES Tipo_Entreno(id_tipo_entreno);

alter table Miembro_Trabajador
ADD FOREIGN KEY (id_trabajador) REFERENCES Trabajador(id_trabajador),
ADD FOREIGN KEY (id_tipo_entreno) REFERENCES Tipo_Entreno(id_tipo_entreno),
ADD FOREIGN KEY (id_miembro) REFERENCES Miembro(id_miembro);

Alter table Horarios_Clase
ADD FOREIGN KEY (id_entrenador) REFERENCES Trabajador(id_trabajador),
ADD FOREIGN KEY (id_tipo_entreno) REFERENCES Tipo_Entreno(id_tipo_entreno);

-- Procedimientos
DELIMITER //

CREATE PROCEDURE RegistrarMiembro(
    IN p_nombre NVARCHAR(30),
    IN p_apellido_p NVARCHAR(20),
    IN p_apellido_m NVARCHAR(20),
    IN p_ine CHAR(13),
    IN p_telefono VARCHAR(15),
    IN p_correo_e NVARCHAR(40),
    IN p_activo BOOL,
    IN p_matricula CHAR(9),
    IN p_id_membresia int
)
BEGIN
    INSERT INTO Miembro (nombre, apellido_p, apellido_m, ine, telefono, correo_e, activo, matricula, id_membresia)
    VALUES (p_nombre, p_apellido_p, p_apellido_m, p_ine, p_telefono, p_correo_e, p_activo, p_matricula, p_id_membresia);
END //

CREATE PROCEDURE InsertarTrabajador(
    IN p_nombre NVARCHAR(30),
    IN p_apellido_p NVARCHAR(20),
    IN p_apellido_m NVARCHAR(20),
    IN p_INE CHAR(13),
    IN p_telefono VARCHAR(15),
    IN p_correo_e NVARCHAR(40),
	OUT p_id_trabajador INT
)
BEGIN
    declare matricula_generada char(9);
    declare matricula_existente int;
    set matricula_generada = GenerarMatricula();
    select count(*) into matricula_existente from Trabajador where matricula = matricula_generada;
    while matricula_existente > 0 do
        set matricula_generada = GenerarMatricula();
        select count(*) into matricula_existente from Trabajador where matricula = matricula_generada;
    end while;
    insert into Trabajador(nombre,apellido_p,apellido_m,ine,telefono,correo_e,matricula)
    values(p_nombre,p_apellido_p,p_apellido_m,p_INE,p_telefono,p_correo_e,matricula_generada);
END //

CREATE FUNCTION GenerarMatricula() RETURNS char(9) DETERMINISTIC
BEGIN
    declare caracteres varchar(37);
    declare i int default 1;
    declare matricula varchar(9) default '';
    set caracteres = '0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ';
    while i <= 9 DO
        set matricula = concat(matricula, substring(caracteres, FLOOR(1 + rand() * 36),1));
        set i = i + 1;
    end while;
    return matricula;
END //

DELIMITER ;

-- Inserciones adicionales y llamadas a procedimientos
CALL RegistrarMiembro('pedro', 'flores', 'gomez', '3jud3njd3knnk', '9994536353', 'pedro@example.com', true, '32djkmed', 1);
CALL InsertarTrabajador('fer', 'zavala', 'gom', '1a', '7878', 'aka@');

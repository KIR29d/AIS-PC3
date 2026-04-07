-- =============================================
-- АИС Магазина Кастомных ПК: Схема БД для MySQL
-- Совместимость: MySQL 8.0.16+ / MariaDB 10.2.1+
-- Кодировка: utf8mb4 (поддержка кириллицы и эмодзи)
-- =============================================

CREATE DATABASE IF NOT EXISTS `ais_custom_pc`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `ais_custom_pc`;

-- Временное отключение проверки внешних ключей для безопасного DROP/CREATE
SET FOREIGN_KEY_CHECKS = 0;

-- =============================================
-- 1. Справочники
-- =============================================

DROP TABLE IF EXISTS `аудит_изменений`;
DROP TABLE IF EXISTS `компоненты_в_сборке`;
DROP TABLE IF EXISTS `сборочные_задания`;
DROP TABLE IF EXISTS `остатки_на_складе`;
DROP TABLE IF EXISTS `заказы`;
DROP TABLE IF EXISTS `пользователи`;
DROP TABLE IF EXISTS `клиенты`;
DROP TABLE IF EXISTS `сотрудники`;
DROP TABLE IF EXISTS `компоненты`;
DROP TABLE IF EXISTS `склады`;
DROP TABLE IF EXISTS `отделы`;
DROP TABLE IF EXISTS `роли`;

CREATE TABLE `роли` (
  `роль_id` INT NOT NULL AUTO_INCREMENT,
  `название_роли` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`роль_id`),
  UNIQUE KEY `uk_роли_название` (`название_роли`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `отделы` (
  `отдел_id` INT NOT NULL AUTO_INCREMENT,
  `название` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`отдел_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `склады` (
  `склад_id` INT NOT NULL AUTO_INCREMENT,
  `название` VARCHAR(100) NOT NULL,
  `адрес` TEXT NOT NULL,
  PRIMARY KEY (`склад_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `компоненты` (
  `компонент_id` INT NOT NULL AUTO_INCREMENT,
  `наименование` VARCHAR(255) NOT NULL,
  `тип` VARCHAR(50) NOT NULL,
  `производитель` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`компонент_id`),
  INDEX `idx_компоненты_тип` (`тип`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================
-- 2. Сущности
-- =============================================

CREATE TABLE `клиенты` (
  `клиент_id` INT NOT NULL AUTO_INCREMENT,
  `тип_клиента` VARCHAR(20) NOT NULL CHECK (`тип_клиента` IN ('физическое', 'юридическое')),
  `фамилия` VARCHAR(100) DEFAULT NULL,
  `имя` VARCHAR(100) DEFAULT NULL,
  `отчество` VARCHAR(100) DEFAULT NULL,
  `наименование_организации` VARCHAR(255) DEFAULT NULL,
  `email` VARCHAR(255) NOT NULL,
  `номер_телефона` VARCHAR(20) NOT NULL,
  `дата_регистрации` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`клиент_id`),
  UNIQUE KEY `uk_клиенты_email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `сотрудники` (
  `сотрудник_id` INT NOT NULL AUTO_INCREMENT,
  `фамилия` VARCHAR(100) NOT NULL,
  `имя` VARCHAR(100) NOT NULL,
  `отчество` VARCHAR(100) DEFAULT NULL,
  `должность` VARCHAR(100) NOT NULL,
  `дата_приёма` DATE NOT NULL,
  `отдел_id` INT NOT NULL,
  PRIMARY KEY (`сотрудник_id`),
  CONSTRAINT `fk_сотрудники_отдел_id` FOREIGN KEY (`отдел_id`) REFERENCES `отделы` (`отдел_id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `пользователи` (
  `пользователь_id` INT NOT NULL AUTO_INCREMENT,
  `логин` VARCHAR(100) NOT NULL,
  `пароль` VARCHAR(255) NOT NULL,
  `роль_id` INT NOT NULL,
  `сотрудник_id` INT DEFAULT NULL,
  `клиент_id` INT DEFAULT NULL,
  `активен` TINYINT(1) NOT NULL DEFAULT 1,
  `дата_создания` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `дата_последнего_входа` DATETIME DEFAULT NULL,
  PRIMARY KEY (`пользователь_id`),
  UNIQUE KEY `uk_пользователи_логин` (`логин`),
  CONSTRAINT `fk_пользователи_роль_id` FOREIGN KEY (`роль_id`) REFERENCES `роли` (`роль_id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_пользователи_сотрудник_id` FOREIGN KEY (`сотрудник_id`) REFERENCES `сотрудники` (`сотрудник_id`) ON DELETE SET NULL,
  CONSTRAINT `fk_пользователи_клиент_id` FOREIGN KEY (`клиент_id`) REFERENCES `клиенты` (`клиент_id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `заказы` (
  `заказ_id` INT NOT NULL AUTO_INCREMENT,
  `клиент_id` INT NOT NULL,
  `статус` VARCHAR(50) NOT NULL CHECK (`статус` IN ('новый', 'в_работе', 'собран', 'отгружен', 'выполнен')),
  `общая_сумма` DECIMAL(12,2) NOT NULL DEFAULT 0.00,
  `дата_создания` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `дата_отгрузки` DATETIME DEFAULT NULL,
  PRIMARY KEY (`заказ_id`),
  CONSTRAINT `fk_заказы_клиент_id` FOREIGN KEY (`клиент_id`) REFERENCES `клиенты` (`клиент_id`) ON DELETE RESTRICT,
  INDEX `idx_заказы_статус` (`статус`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `сборочные_задания` (
  `сборка_id` INT NOT NULL AUTO_INCREMENT,
  `название_конфигурации` VARCHAR(255) NOT NULL,
  `статус` VARCHAR(50) NOT NULL CHECK (`статус` IN ('ожидает', 'в_сборке', 'готово')),
  `сотрудник_id` INT DEFAULT NULL,
  `заказ_id` INT DEFAULT NULL,
  `дата_начала` DATETIME DEFAULT NULL,
  `дата_завершения` DATETIME DEFAULT NULL,
  PRIMARY KEY (`сборка_id`),
  CONSTRAINT `fk_сборочные_сотрудник_id` FOREIGN KEY (`сотрудник_id`) REFERENCES `сотрудники` (`сотрудник_id`) ON DELETE SET NULL,
  CONSTRAINT `fk_сборочные_заказ_id` FOREIGN KEY (`заказ_id`) REFERENCES `заказы` (`заказ_id`) ON DELETE SET NULL,
  INDEX `idx_сборочные_статус` (`статус`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `компоненты_в_сборке` (
  `сборка_id` INT NOT NULL,
  `компонент_id` INT NOT NULL,
  `количество` INT NOT NULL,
  PRIMARY KEY (`сборка_id`, `компонент_id`),
  CONSTRAINT `fk_компоненты_в_сборке_сборка_id` FOREIGN KEY (`сборка_id`) REFERENCES `сборочные_задания` (`сборка_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_компоненты_в_сборке_компонент_id` FOREIGN KEY (`компонент_id`) REFERENCES `компоненты` (`компонент_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `остатки_на_складе` (
  `компонент_id` INT NOT NULL,
  `склад_id` INT NOT NULL,
  `количество` INT NOT NULL CHECK (`количество` >= 0),
  `дата_обновления` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`компонент_id`, `склад_id`),
  CONSTRAINT `fk_остатки_компонент_id` FOREIGN KEY (`компонент_id`) REFERENCES `компоненты` (`компонент_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_остатки_склад_id` FOREIGN KEY (`склад_id`) REFERENCES `склады` (`склад_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE `аудит_изменений` (
  `аудит_id` INT NOT NULL AUTO_INCREMENT,
  `таблица` VARCHAR(100) NOT NULL,
  `операция` VARCHAR(10) NOT NULL CHECK (`операция` IN ('INSERT', 'UPDATE', 'DELETE')),
  `пользователь_id` INT NOT NULL,
  `дата_изменения` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `детали` TEXT NOT NULL,
  PRIMARY KEY (`аудит_id`),
  CONSTRAINT `fk_аудит_пользователь_id` FOREIGN KEY (`пользователь_id`) REFERENCES `пользователи` (`пользователь_id`) ON DELETE RESTRICT,
  INDEX `idx_аудит_дата` (`дата_изменения`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET FOREIGN_KEY_CHECKS = 1;

-- =============================================
-- Начальные данные (Seed)
-- =============================================
INSERT IGNORE INTO `роли` (`название_роли`) VALUES 
('Администратор'), ('Менеджер'), ('Сборщик'), ('Клиент');

INSERT IGNORE INTO `отделы` (`название`) VALUES 
('Отдел продаж'), ('Отдел сборки'), ('Склад и логистика');

INSERT IGNORE INTO `склады` (`название`, `адрес`) VALUES 
('Основной склад комплектующих', 'г. Москва, ул. Компьютерная, д. 10, стр. 3');

-- ⚠️ ВАЖНО: Пароли должны храниться только в захешированном виде (bcrypt/Argon2).
-- Ниже пример хеша для пароля "Admin123!". В C# используйте BCrypt.Net-Next.
INSERT IGNORE INTO `пользователи` (`логин`, `пароль`, `роль_id`, `активен`) VALUES 
('admin', '$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi', 
 (SELECT `роль_id` FROM `роли` WHERE `название_роли`='Администратор' LIMIT 1), 1);
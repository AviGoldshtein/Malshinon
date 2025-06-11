
-- SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
-- START TRANSACTION;
-- SET time_zone = "+00:00";


CREATE DATABASE IF NOT EXISTS malshinonDB;
USE malshinonDB;


CREATE TABLE IF NOT EXISTS `people` (
  `id` INT(11) PRIMARY KEY AUTO_INCREMENT,
  `first_name` VARCHAR(50) NOT NULL UNIQUE,
  `last_name` VARCHAR(50) NOT NULL,
  `secret_code` VARCHAR(50) NOT NULL UNIQUE,
  `type` ENUM('reporter', 'target', 'both', 'potential_agent'),
  `num_reports` INT DEFAULT 0,
  `num_mentions` INT DEFAULT 0
);

CREATE TABLE IF NOT EXISTS `IntelReports` (
  `id` INT(11) PRIMARY KEY AUTO_INCREMENT,
  `reporter_id` INT,
  `target_id` INT,
  `text` TEXT,
  `timestamp` DATETIME DEFAULT NOW(),
  FOREIGN KEY (`reporter_id`) REFERENCES `people`(`id`) ON DELETE CASCADE,
  FOREIGN KEY (`target_id`) REFERENCES `people`(`id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `alerts` (
  `id` INT(11) PRIMARY KEY AUTO_INCREMENT,
  `target_id` INT,
  `reason` TEXT,
  `created_at` DATETIME DEFAULT NOW(),
  FOREIGN KEY (`target_id`) REFERENCES `people`(`id`) ON DELETE CASCADE
);

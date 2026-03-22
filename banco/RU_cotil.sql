CREATE DATABASE  IF NOT EXISTS `cl203271` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cl203271`;
-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 143.106.241.4    Database: cl203271
-- ------------------------------------------------------
-- Server version	8.0.32

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `avaliacoes`
--

DROP TABLE IF EXISTS `avaliacoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `avaliacoes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_user` int NOT NULL,
  `id_pratos` int NOT NULL,
  `avaliacao` varchar(255) DEFAULT NULL,
  `nota` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_user_idx` (`id_user`),
  KEY `id_pratos_idx` (`id_pratos`),
  CONSTRAINT `id_pratos_avaliado` FOREIGN KEY (`id_pratos`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_user_avaliando` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avaliacoes`
--

LOCK TABLES `avaliacoes` WRITE;
/*!40000 ALTER TABLE `avaliacoes` DISABLE KEYS */;
/*!40000 ALTER TABLE `avaliacoes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cardapio`
--

DROP TABLE IF EXISTS `cardapio`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cardapio` (
  `id` int NOT NULL AUTO_INCREMENT,
  `vegano` tinyint NOT NULL,
  `id_acompanhamento` int DEFAULT NULL,
  `id_prato_principal` int DEFAULT NULL,
  `id_guarnicao` int DEFAULT NULL,
  `id_salada` int DEFAULT NULL,
  `id_sobremesa` int DEFAULT NULL,
  `id_refresco` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_acompanhamento_idx` (`id_acompanhamento`),
  KEY `id_principal_idx` (`id_prato_principal`),
  KEY `id_guarnicao_idx` (`id_guarnicao`),
  KEY `id_salada_idx` (`id_salada`),
  KEY `id_sobremesa_idx` (`id_sobremesa`),
  KEY `id_refresco_idx` (`id_refresco`),
  CONSTRAINT `id_acompanhamento` FOREIGN KEY (`id_acompanhamento`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_guarnicao` FOREIGN KEY (`id_guarnicao`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_principal` FOREIGN KEY (`id_prato_principal`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_refresco` FOREIGN KEY (`id_refresco`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_salada` FOREIGN KEY (`id_salada`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_sobremesa` FOREIGN KEY (`id_sobremesa`) REFERENCES `pratos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cardapio`
--

LOCK TABLES `cardapio` WRITE;
/*!40000 ALTER TABLE `cardapio` DISABLE KEYS */;
/*!40000 ALTER TABLE `cardapio` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cardapio_dia`
--

DROP TABLE IF EXISTS `cardapio_dia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cardapio_dia` (
  `id` int NOT NULL AUTO_INCREMENT,
  `data` date NOT NULL,
  `id_padrao_almoco` int DEFAULT NULL,
  `id_vegano_almoco` int DEFAULT NULL,
  `id_padrao_jantar` int DEFAULT NULL,
  `id_vegano_jantar` int DEFAULT NULL,
  `id_user` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_user_cardapio_dia_idx` (`id_user`),
  KEY `id_vegano_jantar_idx` (`id_vegano_jantar`),
  KEY `id_padrao_jantar_idx` (`id_padrao_jantar`),
  KEY `id_vegano_almoco_idx` (`id_vegano_almoco`),
  KEY `id_padrao_almoco_idx` (`id_padrao_almoco`),
  CONSTRAINT `id_padrao_almoco` FOREIGN KEY (`id_padrao_almoco`) REFERENCES `cardapio` (`id`),
  CONSTRAINT `id_padrao_jantar` FOREIGN KEY (`id_padrao_jantar`) REFERENCES `cardapio` (`id`),
  CONSTRAINT `id_user_cardapio_dia` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`),
  CONSTRAINT `id_vegano_almoco` FOREIGN KEY (`id_vegano_almoco`) REFERENCES `cardapio` (`id`),
  CONSTRAINT `id_vegano_jantar` FOREIGN KEY (`id_vegano_jantar`) REFERENCES `cardapio` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cardapio_dia`
--

LOCK TABLES `cardapio_dia` WRITE;
/*!40000 ALTER TABLE `cardapio_dia` DISABLE KEYS */;
/*!40000 ALTER TABLE `cardapio_dia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoria` (
  `id` int NOT NULL AUTO_INCREMENT,
  `descricao` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `descricao_UNIQUE` (`descricao`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
/*!40000 ALTER TABLE `categoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificacoes`
--

DROP TABLE IF EXISTS `notificacoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificacoes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_user` int NOT NULL,
  `titulo` varchar(45) NOT NULL,
  `descricao` varchar(255) DEFAULT NULL,
  `model` varchar(45) NOT NULL,
  `data_inicial` date NOT NULL,
  `data_final` date NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_user_idx` (`id_user`),
  CONSTRAINT `id_user` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacoes`
--

LOCK TABLES `notificacoes` WRITE;
/*!40000 ALTER TABLE `notificacoes` DISABLE KEYS */;
/*!40000 ALTER TABLE `notificacoes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pratos`
--

DROP TABLE IF EXISTS `pratos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pratos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `descricao` varchar(255) NOT NULL,
  `vegano` tinyint NOT NULL,
  `imagem` varchar(255) DEFAULT NULL,
  `id_categoria` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nome_UNIQUE` (`nome`),
  KEY `id_categoria_idx` (`id_categoria`),
  CONSTRAINT `id_categoria` FOREIGN KEY (`id_categoria`) REFERENCES `categoria` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pratos`
--

LOCK TABLES `pratos` WRITE;
/*!40000 ALTER TABLE `pratos` DISABLE KEYS */;
/*!40000 ALTER TABLE `pratos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pratos_favoritados`
--

DROP TABLE IF EXISTS `pratos_favoritados`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pratos_favoritados` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_pratos` int NOT NULL,
  `id_user` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_pratos_favoritado_idx` (`id_pratos`),
  KEY `id_user_favoritando_idx` (`id_user`),
  CONSTRAINT `id_pratos_favoritado` FOREIGN KEY (`id_pratos`) REFERENCES `pratos` (`id`),
  CONSTRAINT `id_user_favoritando` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pratos_favoritados`
--

LOCK TABLES `pratos_favoritados` WRITE;
/*!40000 ALTER TABLE `pratos_favoritados` DISABLE KEYS */;
/*!40000 ALTER TABLE `pratos_favoritados` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `login` varchar(255) DEFAULT NULL,
  `nome` varchar(255) DEFAULT NULL,
  `senha_hash` varchar(255) NOT NULL,
  `funcionario` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (7,'dev','Felipe Ferreira','102030',_binary '');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `valor_nutricional`
--

DROP TABLE IF EXISTS `valor_nutricional`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `valor_nutricional` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_prato` int NOT NULL,
  `medida` varchar(45) NOT NULL,
  `kcal` float NOT NULL,
  `carboidratos` float NOT NULL,
  `proteinas` float NOT NULL,
  `lipidios` float NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_prato_UNIQUE` (`id_prato`),
  CONSTRAINT `id_pratos` FOREIGN KEY (`id_prato`) REFERENCES `pratos` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `valor_nutricional`
--

LOCK TABLES `valor_nutricional` WRITE;
/*!40000 ALTER TABLE `valor_nutricional` DISABLE KEYS */;
/*!40000 ALTER TABLE `valor_nutricional` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-22 19:46:40

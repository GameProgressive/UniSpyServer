-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versione server:              10.2.14-MariaDB - mariadb.org binary distribution
-- S.O. server:                  Win64
-- HeidiSQL Versione:            9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dump della struttura del database retrospy
CREATE DATABASE IF NOT EXISTS `retrospy` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `retrospy`;

-- Dump della struttura di tabella retrospy.addrequests
CREATE TABLE IF NOT EXISTS `addrequests` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `profileid` int(10) unsigned NOT NULL DEFAULT 0,
  `targetid` int(11) unsigned NOT NULL,
  `syncrequested` varchar(255) NOT NULL DEFAULT '',
  `reason` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FK_addrequests_profiles` (`profileid`),
  KEY `FK_addrequests_profiles_2` (`targetid`),
  CONSTRAINT `FK_addrequests_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`),
  CONSTRAINT `FK_addrequests_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.addrequests: ~0 rows (circa)
DELETE FROM `addrequests`;
/*!40000 ALTER TABLE `addrequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `addrequests` ENABLE KEYS */;

-- Dump della struttura di tabella retrospy.blocked
CREATE TABLE IF NOT EXISTS `blocked` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `targetid` int(10) unsigned NOT NULL,
  `profileid` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FK_blocked_profiles` (`profileid`),
  KEY `FK_blocked_profiles_2` (`targetid`),
  CONSTRAINT `FK_blocked_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`),
  CONSTRAINT `FK_blocked_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.blocked: ~0 rows (circa)
DELETE FROM `blocked`;
/*!40000 ALTER TABLE `blocked` DISABLE KEYS */;
/*!40000 ALTER TABLE `blocked` ENABLE KEYS */;

-- Dump della struttura di tabella retrospy.friends
CREATE TABLE IF NOT EXISTS `friends` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `profileid` int(10) unsigned NOT NULL DEFAULT 0,
  `targetid` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FK_friends_profiles` (`profileid`),
  KEY `FK_friends_profiles_2` (`targetid`),
  CONSTRAINT `FK_friends_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`),
  CONSTRAINT `FK_friends_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.friends: ~0 rows (circa)
DELETE FROM `friends`;
/*!40000 ALTER TABLE `friends` DISABLE KEYS */;
/*!40000 ALTER TABLE `friends` ENABLE KEYS */;

-- Dump della struttura di tabella retrospy.messages
CREATE TABLE IF NOT EXISTS `messages` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `from` int(10) unsigned NOT NULL,
  `to` int(10) unsigned NOT NULL,
  `date` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `message` varchar(200) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FK_messages_profiles` (`from`),
  KEY `FK_messages_profiles_2` (`to`),
  CONSTRAINT `FK_messages_profiles` FOREIGN KEY (`from`) REFERENCES `profiles` (`profileid`),
  CONSTRAINT `FK_messages_profiles_2` FOREIGN KEY (`to`) REFERENCES `profiles` (`profileid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.messages: ~0 rows (circa)
DELETE FROM `messages`;
/*!40000 ALTER TABLE `messages` DISABLE KEYS */;
/*!40000 ALTER TABLE `messages` ENABLE KEYS */;

-- Dump della struttura di tabella retrospy.profiles
CREATE TABLE IF NOT EXISTS `profiles` (
  `profileid` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `userid` int(11) unsigned NOT NULL DEFAULT 0,
  `sesskey` int(11) DEFAULT NULL,
  `uniquenick` varchar(20) NOT NULL DEFAULT '''''',
  `nick` varchar(30) NOT NULL DEFAULT '''''',
  `firstname` varchar(30) NOT NULL DEFAULT '''''',
  `lastname` varchar(30) NOT NULL DEFAULT '''''',
  `publicmask` int(11) NOT NULL DEFAULT 0,
  `deleted` tinyint(1) NOT NULL DEFAULT 0,
  `latitude` float NOT NULL DEFAULT 0,
  `longitude` float NOT NULL DEFAULT 0,
  `aim` varchar(50) DEFAULT '0',
  `picture` int(11) DEFAULT 0,
  `occupationid` int(11) DEFAULT 0,
  `incomeid` int(11) DEFAULT 0,
  `industryid` int(11) DEFAULT 0,
  `marriedid` int(11) DEFAULT 0,
  `childcount` int(11) DEFAULT 0,
  `interests1` int(11) DEFAULT 0,
  `ownership1` int(11) DEFAULT 0,
  `connectiontype` int(11) DEFAULT 0,
  `sex` enum('MALE','FEMALE','PAT') DEFAULT 'PAT',
  `zipcode` varchar(10) DEFAULT '00000',
  `countrycode` varchar(2) DEFAULT '0',
  `homepage` varchar(75) DEFAULT '0',
  `birthday` int(11) DEFAULT 0,
  `birthmonth` int(11) DEFAULT 0,
  `birthyear` int(11) DEFAULT 0,
  `location` varchar(127) DEFAULT '0',
  `icq` int(11) DEFAULT 0,
  PRIMARY KEY (`profileid`),
  UNIQUE KEY `profileid` (`profileid`),
  UNIQUE KEY `uniquenick` (`uniquenick`),
  UNIQUE KEY `sesskey` (`sesskey`),
  KEY `FK_profiles_users` (`userid`),
  CONSTRAINT `FK_profiles_users` FOREIGN KEY (`userid`) REFERENCES `users` (`userid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.profiles: ~1 rows (circa)
DELETE FROM `profiles`;
/*!40000 ALTER TABLE `profiles` DISABLE KEYS */;
INSERT INTO `profiles` (`profileid`, `userid`, `sesskey`, `uniquenick`, `nick`, `firstname`, `lastname`, `publicmask`, `deleted`, `latitude`, `longitude`, `aim`, `picture`, `occupationid`, `incomeid`, `industryid`, `marriedid`, `childcount`, `interests1`, `ownership1`, `connectiontype`, `sex`, `zipcode`, `countrycode`, `homepage`, `birthday`, `birthmonth`, `birthyear`, `location`, `icq`) VALUES
	(2, 1, NULL, 'SpyGuy', 'SpyGuy', 'Spy', 'Guy', 0, 0, 40.7142, -74.0064, 'spyguy@aim.com', 0, 0, 0, 0, 0, 0, 0, 0, 3, 'MALE', '10001', 'US', 'https://www.gamespy.com/', 20, 3, 1980, 'New York', 0);
/*!40000 ALTER TABLE `profiles` ENABLE KEYS */;

-- Dump della struttura di tabella retrospy.users
CREATE TABLE IF NOT EXISTS `users` (
  `userid` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `email` varchar(50) NOT NULL,
  `password` varchar(32) NOT NULL,
  `status` smallint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`userid`),
  UNIQUE KEY `userid` (`userid`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Dump dei dati della tabella retrospy.users: ~1 rows (circa)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`userid`, `email`, `password`, `status`) VALUES
	(1, 'spyguy@gamespy.com', '0000', 1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

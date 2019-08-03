/*
 Navicat Premium Data Transfer

 Source Server         : MariaDB 10
 Source Server Type    : MySQL
 Source Server Version : 100316
 Source Host           : localhost:3316
 Source Schema         : retrospy

 Target Server Type    : MySQL
 Target Server Version : 100316
 File Encoding         : 65001

 Date: 06/07/2019 21:44:16
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for addrequests
-- ----------------------------
DROP TABLE IF EXISTS `addrequests`;
CREATE TABLE `addrequests`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `profileid` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `targetid` int(11) UNSIGNED NOT NULL,
  `syncrequested` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `reason` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id`) USING BTREE,
  INDEX `FK_addrequests_profiles`(`profileid`) USING BTREE,
  INDEX `FK_addrequests_profiles_2`(`targetid`) USING BTREE,
  CONSTRAINT `FK_addrequests_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_addrequests_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for blocked
-- ----------------------------
DROP TABLE IF EXISTS `blocked`;
CREATE TABLE `blocked`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `targetid` int(10) UNSIGNED NOT NULL,
  `profileid` int(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id`) USING BTREE,
  INDEX `FK_blocked_profiles`(`profileid`) USING BTREE,
  INDEX `FK_blocked_profiles_2`(`targetid`) USING BTREE,
  CONSTRAINT `FK_blocked_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_blocked_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for friends
-- ----------------------------
DROP TABLE IF EXISTS `friends`;
CREATE TABLE `friends`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `profileid` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `targetid` int(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id`) USING BTREE,
  INDEX `FK_friends_profiles`(`profileid`) USING BTREE,
  INDEX `FK_friends_profiles_2`(`targetid`) USING BTREE,
  CONSTRAINT `FK_friends_profiles` FOREIGN KEY (`profileid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_friends_profiles_2` FOREIGN KEY (`targetid`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for messages
-- ----------------------------
DROP TABLE IF EXISTS `messages`;
CREATE TABLE `messages`  (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `from` int(10) UNSIGNED NOT NULL,
  `to` int(10) UNSIGNED NOT NULL,
  `date` timestamp(0) NOT NULL DEFAULT current_timestamp(0) ON UPDATE CURRENT_TIMESTAMP(0),
  `message` varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `id`(`id`) USING BTREE,
  INDEX `FK_messages_profiles`(`from`) USING BTREE,
  INDEX `FK_messages_profiles_2`(`to`) USING BTREE,
  CONSTRAINT `FK_messages_profiles` FOREIGN KEY (`from`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_messages_profiles_2` FOREIGN KEY (`to`) REFERENCES `profiles` (`profileid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for profiles
-- ----------------------------
DROP TABLE IF EXISTS `profiles`;
CREATE TABLE `profiles`  (
  `profileid` int(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `userid` int(11) UNSIGNED NOT NULL DEFAULT 0,
  `uniquenick` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `nick` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `firstname` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `lastname` varchar(30) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `publicmask` int(11) NOT NULL DEFAULT 0,
  `latitude` float(10, 0) NOT NULL DEFAULT 0,
  `longitude` float(10, 0) NOT NULL DEFAULT 0,
  `aim` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  `picture` int(11) NULL DEFAULT 0,
  `occupationid` int(11) NULL DEFAULT 0,
  `incomeid` int(11) NULL DEFAULT 0,
  `industryid` int(11) NULL DEFAULT 0,
  `marriedid` int(11) NULL DEFAULT 0,
  `childcount` int(11) NULL DEFAULT 0,
  `interests1` int(11) NULL DEFAULT 0,
  `ownership1` int(11) NULL DEFAULT 0,
  `connectiontype` int(11) NULL DEFAULT 0,
  `sex` enum('MALE','FEMALE','PAT') CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT 'PAT',
  `zipcode` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '00000',
  `countrycode` varchar(2) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `homepage` varchar(75) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `birthday` int(2) NULL DEFAULT 0,
  `birthmonth` int(2) NULL DEFAULT 0,
  `birthyear` int(4) NULL DEFAULT 0,
  `location` varchar(127) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '',
  `icq` int(8) NULL DEFAULT 0,
  `status` tinyint(4) NOT NULL DEFAULT 0,
  `lastip` varchar(16) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `lastonline` int(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`profileid`) USING BTREE,
  UNIQUE INDEX `profileid`(`profileid`) USING BTREE,
  UNIQUE INDEX `uniquenick`(`uniquenick`) USING BTREE,
  INDEX `FK_profiles_users`(`userid`) USING BTREE,
  CONSTRAINT `FK_profiles_users` FOREIGN KEY (`userid`) REFERENCES `users` (`userid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of profiles
-- ----------------------------
INSERT INTO `profiles` VALUES (1, 1, 'SpyGuy', 'SpyGuy', 'Spy', 'Guy', 0, 41, -74, 'spyguy@aim.com', 0, 0, 0, 0, 0, 0, 0, 0, 3, 'MALE', '10001', 'US', 'https://www.gamespy.com/', 20, 3, 1980, 'New York', 0, 0, '127.0.0.1', 1562442207);
INSERT INTO `profiles` VALUES (2, 2, 'SpyGuy2', 'SpyGuy2', 'Spy', 'Guy', 0, 0, 0, '0', 0, 0, 0, 0, 0, 0, 0, 0, 0, 'PAT', '00000', '', '', 0, 0, 0, '', 0, 0, '127.0.0.1', 1562441613);

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `userid` int(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `email` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `userstatus` smallint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`userid`) USING BTREE,
  UNIQUE INDEX `userid`(`userid`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'spyguy@gamespy.com', '4a7d1ed414474e4033ac29ccb8653d9b', 1);
INSERT INTO `users` VALUES (2, 'spyguy2@gamespy.com', '4a7d1ed414474e4033ac29ccb8653d9b', 1);

SET FOREIGN_KEY_CHECKS = 1;

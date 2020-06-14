CREATE TABLE `user` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Username` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  `Password` varchar(512) COLLATE utf8mb4_bin DEFAULT NULL,
  `LastLogedin` datetime DEFAULT NULL,
  `LastToken` varchar(512) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

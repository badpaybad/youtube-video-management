CREATE TABLE `cmscategory` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Title` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
CREATE TABLE `cmscontent` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Title` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  `Thumbnail` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  `Description` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  `UrlRef` varchar(2048) COLLATE utf8mb4_bin DEFAULT NULL,
  `CountView` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
CREATE TABLE `cmsrelation` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CategoryId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `ContentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
CREATE TABLE `sysmodule` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Code` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
CREATE TABLE `syspermission` (
  `Id` varchar(36) COLLATE utf8mb4_bin NOT NULL,
  `ParentId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `TenantId` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsDeleted` int(11) DEFAULT NULL,
  `CreatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `UpdatedBy` varchar(36) COLLATE utf8mb4_bin DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `Code` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  `ModuleCode` varchar(128) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
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


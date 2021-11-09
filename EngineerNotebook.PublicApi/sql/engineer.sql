CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20211109032150_initcreate')
BEGIN
    CREATE TABLE `Docs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` text NULL,
        `Description` text NULL,
        `Contents` text NULL,
        `CreatedByUserId` text NULL,
        `EditedByUserId` text NULL,
        `CreatedAt` timestamp NOT NULL,
        `EditedAt` timestamp NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20211109032150_initcreate')
BEGIN
    CREATE TABLE `Tags` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` text NULL,
        `TagType` int NOT NULL,
        PRIMARY KEY (`Id`)
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20211109032150_initcreate')
BEGIN
    CREATE TABLE `DocumentationTag` (
        `DocsId` int NOT NULL,
        `TagsId` int NOT NULL,
        PRIMARY KEY (`DocsId`, `TagsId`),
        CONSTRAINT `FK_DocumentationTag_Docs_DocsId` FOREIGN KEY (`DocsId`) REFERENCES `Docs` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_DocumentationTag_Tags_TagsId` FOREIGN KEY (`TagsId`) REFERENCES `Tags` (`Id`) ON DELETE CASCADE
    );
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20211109032150_initcreate')
BEGIN
    CREATE INDEX `IX_DocumentationTag_TagsId` ON `DocumentationTag` (`TagsId`);
END;

IF NOT EXISTS(SELECT * FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20211109032150_initcreate')
BEGIN
    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20211109032150_initcreate', '5.0.11');
END;

COMMIT;


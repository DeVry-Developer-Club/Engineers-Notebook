CREATE TABLE IF NOT EXISTS `Docs` (
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

CREATE TABLE IF NOT EXISTS `Tags` (
	`Id` int NOT NULL AUTO_INCREMENT,
	`Name` text NULL,
	`TagType` int NOT NULL,
	PRIMARY KEY (`Id`)
);

CREATE TABLE IF NOT EXISTS `DocumentationTag` (
	`DocsId` int NOT NULL,
	`TagsId` int NOT NULL,
	PRIMARY KEY (`DocsId`, `TagsId`),
    INDEX(`TagsId`),
	CONSTRAINT `FK_DocumentationTag_Docs_DocsId` FOREIGN KEY (`DocsId`) REFERENCES `Docs` (`Id`) ON DELETE CASCADE,
	CONSTRAINT `FK_DocumentationTag_Tags_TagsId` FOREIGN KEY (`TagsId`) REFERENCES `Tags` (`Id`) ON DELETE CASCADE
);

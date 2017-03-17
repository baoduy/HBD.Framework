CREATE TABLE [dbo].[Sample]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(500) NOT NULL, 
    [Date] DATETIME NULL, 
    [CreatedBy] VARCHAR(150) NOT NULL, 
    [CreatedTime] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdatedBy] VARCHAR(150) NULL, 
    [UpdatedTime] DATETIME NULL
)

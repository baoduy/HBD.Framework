CREATE TABLE [AuditTrail].[Log]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [TableName] VARCHAR(150) NOT NULL, 
    [AuditType] CHAR NOT NULL, 
    [OldEntry] XML NULL,
	[NewEntry] XML NULL,
    [AuditDate] DATETIME NOT NULL, 
    [UserName] VARCHAR(150) NOT NULL, 
    CONSTRAINT [CK_Log_AuditType] CHECK ([AuditType] IN ('I','U','D'))
)

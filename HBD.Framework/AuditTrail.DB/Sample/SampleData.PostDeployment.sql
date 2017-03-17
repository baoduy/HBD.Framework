/*
Generated Date: 19-Nov-2015 11:35:23
Generated User: SCHRODERSAD\hoangd
*/

PRINT N'Merging static data to [Sample]';
GO

BEGIN TRY
SET IDENTITY_INSERT [Sample] ON
END TRY
BEGIN CATCH
END CATCH
GO

MERGE INTO [Sample] AS Target
USING( VALUES
(1,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(2,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(3,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(4,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(5,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(6,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(7,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(8,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(9,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL),
(10,N'Beverages Soft drinks, coffees, teas, beers, and ales',NULL,N'sa','2015-11-19 12:00:00',NULL,NULL)
)AS Source([Id],[Name],[Date],[CreatedBy],[CreatedTime],[UpdatedBy],[UpdatedTime])
ON Target.[Id] = Source.[Id]
WHEN MATCHED THEN
UPDATE SET [Name] = Source.[Name],
[Date] = Source.[Date],
[CreatedBy] = Source.[CreatedBy],
[CreatedTime] = Source.[CreatedTime],
[UpdatedBy] = Source.[UpdatedBy],
[UpdatedTime] = Source.[UpdatedTime]
WHEN NOT MATCHED BY TARGET THEN
INSERT([Id],[Name],[Date],[CreatedBy],[CreatedTime],[UpdatedBy],[UpdatedTime])
VALUES([Id],[Name],[Date],[CreatedBy],[CreatedTime],[UpdatedBy],[UpdatedTime])
;
GO

BEGIN TRY
SET IDENTITY_INSERT [Sample] OFF
END TRY
BEGIN CATCH
END CATCH
GO

PRINT N'Completed merge static data to Sample';


go

PRINT N'Begin Update static data to [Sample]';
GO

update [Sample] SET Name = 'Testing' where Id <=2;
go

DELETE [Sample] where Id >=7;
go

PRINT N'End Update static data to [Sample]. Please verify the AuditTrail table.';
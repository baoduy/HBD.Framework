/*
Generated Date: 04-Sep-2015 11:15:17
Generated User: SCHRODERSAD\hoangd
*/

PRINT N'Merging static data to [Shippers]';
GO

BEGIN TRY
SET IDENTITY_INSERT [Shippers] ON
END TRY
BEGIN CATCH
END CATCH
GO

MERGE INTO [Shippers] AS Target
USING( VALUES
(1,N'Speedy Express',N'(503) 555-9831'),
(2,N'United Package',N'(503) 555-3199'),
(3,N'Federal Shipping',N'(503) 555-9931')
)AS Source([ShipperID],[CompanyName],[Phone])
ON Target.[ShipperID] = Source.[ShipperID]
WHEN MATCHED THEN
UPDATE SET [CompanyName] = Source.[CompanyName],
[Phone] = Source.[Phone]
WHEN NOT MATCHED BY TARGET THEN
INSERT([ShipperID],[CompanyName],[Phone])
VALUES([ShipperID],[CompanyName],[Phone])
;
GO

BEGIN TRY
SET IDENTITY_INSERT [Shippers] OFF
END TRY
BEGIN CATCH
END CATCH
GO

PRINT N'Completed merge static data to Shippers';


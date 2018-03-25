/*
Generated Date: 04-Sep-2015 11:15:17
Generated User: SCHRODERSAD\hoangd
*/

PRINT N'Merging static data to [Region]';
GO

MERGE INTO [Region] AS Target
USING( VALUES
(1,N'Eastern                                           '),
(2,N'Western                                           '),
(3,N'Northern                                          '),
(4,N'Southern                                          ')
)AS Source([RegionID],[RegionDescription])
ON Target.[RegionID] = Source.[RegionID]
WHEN MATCHED THEN
UPDATE SET [RegionDescription] = Source.[RegionDescription]
WHEN NOT MATCHED BY TARGET THEN
INSERT([RegionID],[RegionDescription])
VALUES([RegionID],[RegionDescription])
;
GO

PRINT N'Completed merge static data to Region';


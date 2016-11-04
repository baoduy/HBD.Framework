/*
Generated Date: 04-Nov-2016 09:36:35
Generated User: STEVEN-PC\Steven
*/

PRINT N'Merging static data to [EmployeeTerritories]';
GO

MERGE INTO [EmployeeTerritories] AS Target
USING( VALUES
(N'1',N'6897',N'',N'',N'',N'',N''),
(N'1',N'19713',N'',N'',N'',N'',N''),
(N'2',N'1581',N'',N'',N'',N'',N''),
(N'2',N'1730',N'',N'',N'',N'',N''),
(N'2',N'1833',N'',N'',N'',N'',N''),
(N'2',N'2116',N'',N'',N'',N'',N''),
(N'2',N'2139',N'',N'',N'',N'',N''),
(N'2',N'2184',N'',N'',N'',N'',N''),
(N'2',N'40222',N'',N'',N'',N'',N''),
(N'3',N'30346',N'',N'',N'',N'',N''),
(N'3',N'31406',N'',N'',N'',N'',N''),
(N'3',N'32859',N'',N'',N'',N'',N''),
(N'3',N'33607',N'',N'',N'',N'',N''),
(N'4',N'20852',N'',N'',N'',N'',N''),
(N'4',N'27403',N'',N'',N'',N'',N''),
(N'4',N'27511',N'',N'',N'',N'',N''),
(N'5',N'2903',N'',N'',N'',N'',N''),
(N'5',N'7960',N'',N'',N'',N'',N''),
(N'5',N'8837',N'',N'',N'',N'',N''),
(N'5',N'10019',N'',N'',N'',N'',N''),
(N'5',N'10038',N'',N'',N'',N'',N''),
(N'5',N'11747',N'',N'',N'',N'',N''),
(N'5',N'14450',N'',N'',N'',N'',N''),
(N'6',N'85014',N'',N'',N'',N'',N''),
(N'6',N'85251',N'',N'',N'',N'',N''),
(N'6',N'98004',N'',N'',N'',N'',N''),
(N'6',N'98052',N'',N'',N'',N'',N''),
(N'6',N'98104',N'',N'',N'',N'',N''),
(N'7',N'60179',N'',N'',N'',N'',N''),
(N'7',N'60601',N'',N'',N'',N'',N''),
(N'7',N'80202',N'',N'',N'',N'',N''),
(N'7',N'80909',N'',N'',N'',N'',N''),
(N'7',N'90405',N'',N'',N'',N'',N''),
(N'7',N'94025',N'',N'',N'',N'',N''),
(N'7',N'94105',N'',N'',N'',N'',N''),
(N'7',N'95008',N'',N'',N'',N'',N''),
(N'7',N'95054',N'',N'',N'',N'',N''),
(N'7',N'95060',N'',N'',N'',N'',N''),
(N'8',N'19428',N'',N'',N'',N'',N''),
(N'8',N'44122',N'',N'',N'',N'',N''),
(N'8',N'45839',N'',N'',N'',N'',N''),
(N'8',N'53404',N'',N'',N'',N'',N''),
(N'9',N'3049',N'',N'',N'',N'',N''),
(N'9',N'3801',N'',N'',N'',N'',N''),
(N'9',N'48075',N'',N'',N'',N'',N''),
(N'9',N'48084',N'',N'',N'',N'',N''),
(N'9',N'48304',N'',N'',N'',N'',N''),
(N'9',N'55113',N'',N'',N'',N'',N''),
(N'9',N'55439',N'',N'',N'',N'',N'')
)AS Source([EmployeeID],[TerritoryID],[Column1],[Column2],[Column3],[Column4],[Column5])
ON 
WHEN MATCHED THEN
UPDATE SET [EmployeeID] = Source.[EmployeeID],
[TerritoryID] = Source.[TerritoryID],
[Column1] = Source.[Column1],
[Column2] = Source.[Column2],
[Column3] = Source.[Column3],
[Column4] = Source.[Column4],
[Column5] = Source.[Column5]
;
GO

PRINT N'Completed merge static data to EmployeeTerritories';


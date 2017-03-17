/*
Generated Date: 04-Sep-2015 11:15:18
Generated User: SCHRODERSAD\hoangd
*/

PRINT N'Merging static data to [Territories]';
GO

MERGE INTO [Territories] AS Target
USING( VALUES
(N'01581',N'Westboro                                          ',1),
(N'01730',N'Bedford                                           ',1),
(N'01833',N'Georgetow                                         ',1),
(N'02116',N'Boston                                            ',1),
(N'02139',N'Cambridge                                         ',1),
(N'02184',N'Braintree                                         ',1),
(N'02903',N'Providence                                        ',1),
(N'03049',N'Hollis                                            ',3),
(N'03801',N'Portsmouth                                        ',3),
(N'06897',N'Wilton                                            ',1),
(N'07960',N'Morristown                                        ',1),
(N'08837',N'Edison                                            ',1),
(N'10019',N'New York                                          ',1),
(N'10038',N'New York                                          ',1),
(N'11747',N'Mellvile                                          ',1),
(N'14450',N'Fairport                                          ',1),
(N'19428',N'Philadelphia                                      ',3),
(N'19713',N'Neward                                            ',1),
(N'20852',N'Rockville                                         ',1),
(N'27403',N'Greensboro                                        ',1),
(N'27511',N'Cary                                              ',1),
(N'29202',N'Columbia                                          ',4),
(N'30346',N'Atlanta                                           ',4),
(N'31406',N'Savannah                                          ',4),
(N'32859',N'Orlando                                           ',4),
(N'33607',N'Tampa                                             ',4),
(N'40222',N'Louisville                                        ',1),
(N'44122',N'Beachwood                                         ',3),
(N'45839',N'Findlay                                           ',3),
(N'48075',N'Southfield                                        ',3),
(N'48084',N'Troy                                              ',3),
(N'48304',N'Bloomfield Hills                                  ',3),
(N'53404',N'Racine                                            ',3),
(N'55113',N'Roseville                                         ',3),
(N'55439',N'Minneapolis                                       ',3),
(N'60179',N'Hoffman Estates                                   ',2),
(N'60601',N'Chicago                                           ',2),
(N'72716',N'Bentonville                                       ',4),
(N'75234',N'Dallas                                            ',4),
(N'78759',N'Austin                                            ',4),
(N'80202',N'Denver                                            ',2),
(N'80909',N'Colorado Springs                                  ',2),
(N'85014',N'Phoenix                                           ',2),
(N'85251',N'Scottsdale                                        ',2),
(N'90405',N'Santa Monica                                      ',2),
(N'94025',N'Menlo Park                                        ',2),
(N'94105',N'San Francisco                                     ',2),
(N'95008',N'Campbell                                          ',2),
(N'95054',N'Santa Clara                                       ',2),
(N'95060',N'Santa Cruz                                        ',2),
(N'98004',N'Bellevue                                          ',2),
(N'98052',N'Redmond                                           ',2),
(N'98104',N'Seattle                                           ',2)
)AS Source([TerritoryID],[TerritoryDescription],[RegionID])
ON Target.[TerritoryID] = Source.[TerritoryID]
WHEN MATCHED THEN
UPDATE SET [TerritoryDescription] = Source.[TerritoryDescription],
[RegionID] = Source.[RegionID]
WHEN NOT MATCHED BY TARGET THEN
INSERT([TerritoryID],[TerritoryDescription],[RegionID])
VALUES([TerritoryID],[TerritoryDescription],[RegionID])
;
GO

PRINT N'Completed merge static data to Territories';


/*
Generated Date: 27-Aug-2015 08:25:14
Generated User: DESKTOP-0DRB3BI\Steven Hoang
*/

PRINT N'Merging static data to [EmployeeTerritories]';
GO

MERGE INTO [EmployeeTerritories] AS Target
USING( VALUES
(1,N'06897'),
(1,N'19713'),
(2,N'01581'),
(2,N'01730'),
(2,N'01833'),
(2,N'02116'),
(2,N'02139'),
(2,N'02184'),
(2,N'40222'),
(3,N'30346'),
(3,N'31406'),
(3,N'32859'),
(3,N'33607'),
(4,N'20852'),
(4,N'27403'),
(4,N'27511'),
(5,N'02903'),
(5,N'07960'),
(5,N'08837'),
(5,N'10019'),
(5,N'10038'),
(5,N'11747'),
(5,N'14450'),
(6,N'85014'),
(6,N'85251'),
(6,N'98004'),
(6,N'98052'),
(6,N'98104'),
(7,N'60179'),
(7,N'60601'),
(7,N'80202'),
(7,N'80909'),
(7,N'90405'),
(7,N'94025'),
(7,N'94105'),
(7,N'95008'),
(7,N'95054'),
(7,N'95060'),
(8,N'19428'),
(8,N'44122'),
(8,N'45839'),
(8,N'53404'),
(9,N'03049'),
(9,N'03801'),
(9,N'48075'),
(9,N'48084'),
(9,N'48304'),
(9,N'55113'),
(9,N'55439')
)AS Source([EmployeeID],[TerritoryID])
ON Target.[EmployeeID] = Source.[EmployeeID] AND Target.[TerritoryID] = Source.[TerritoryID]
WHEN NOT MATCHED BY TARGET THEN
INSERT([EmployeeID],[TerritoryID])
VALUES([EmployeeID],[TerritoryID])
;
GO

PRINT N'Completed merge static data to EmployeeTerritories';


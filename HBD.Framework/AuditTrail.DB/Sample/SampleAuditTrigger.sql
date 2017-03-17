CREATE TRIGGER SampleAuditTrigger
 ON  Sample
 AFTER DELETE, UPDATE
 AS 
 BEGIN
	Declare @type char(1),
		@OldEntry XML,
		@NewEntry XML,
		@UserName nvarchar(150),
		@TableName varchar(100);

	SET @TableName = 'Sample';

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Action & Users.
	IF exists (SELECT TOP 1 * FROM inserted)
	BEGIN
		IF exists (SELECT TOP 1 * FROM deleted) SELECT @type = 'U' ELSE SELECT @type = 'I';
		SELECT @UserName = UpdatedBy FROM inserted;
		IF (@UserName IS NULL)  SET @UserName = SYSTEM_USER;
	END
	ELSE
	BEGIN
		SELECT @type = 'D';
		SELECT @UserName = SYSTEM_USER;--The limitation is UserName can't be identified when delete.
	END

	--Check User again.
	IF (@UserName IS NULL) SET @UserName = CURRENT_USER;

	-- Consolidate Audit Data.
	BEGIN TRY
		IF (SELECT COUNT(*) FROM deleted) > 0 
			set @OldEntry =  (select * from deleted as [Item] for xml AUTO, TYPE, ROOT('Entry'));
		IF (SELECT COUNT(*) FROM inserted) > 0 
			set @NewEntry = (select * from inserted as [Item] for xml AUTO, TYPE, ROOT('Entry'));

		insert into [AuditTrail].[Log](AuditDate, TableName, OldEntry,NewEntry, AuditType, UserName) 
			values (GetDate(), @TableName, @OldEntry,@NewEntry, @type, @UserName);
	END TRY
	BEGIN CATCH
		PRINT 'There is an issue when insert data to [AuditTrail].[Log] table.';
	END CATCH
END
GO



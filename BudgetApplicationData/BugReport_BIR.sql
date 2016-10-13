CREATE TRIGGER [BugReport_BIR]
ON [dbo].[BugReport]
FOR INSERT
AS 
	DECLARE @Id int;
	DECLARE @Username NVARCHAR(50);
	DECLARE @Date DATETIME;
	DECLARE @Description NVARCHAR(300);
	DECLARE @Resolved BIT;
	DECLARE @InProgress BIT;

	SELECT @Username=i.Username FROM inserted i;
	SELECT @Description=i.Description FROM inserted i;

  INSERT INTO dbo.BugReport(Username, Date, Description, Resolved, InProgress)
     VALUES(@Username, SYSUTCDATETIME(), @Description, 0, 0);

	 PRINT 'BUG REPORT AFTER INSERT FIRED'
GO
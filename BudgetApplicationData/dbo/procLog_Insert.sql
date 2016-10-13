
CREATE PROCEDURE [dbo].[procLog_Insert]
	@log_date DATETIME2,
	@log_thread NVARCHAR(50),
	@log_level NVARCHAR(50), 
	@log_source NVARCHAR(300),
	@log_message NVARCHAR(4000),
	@exception NVARCHAR (4000)
AS
BEGIN
SET NOCOUNT ON;

	INSERT INTO [dbo].[Log](logDate, logThread, logLevel, logSource, logMessage, exception)
	VALUES ( @log_date, @log_thread, @log_level, @log_source, @log_message, @exception)

END
GO

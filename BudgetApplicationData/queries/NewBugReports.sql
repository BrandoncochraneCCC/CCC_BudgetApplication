
SELECT ISNULL(Resolved, 0) AS Resolved,ISNULL(InProgress, 0) AS 'In Progress',Username,Date,Description 
	FROM [BugReport]	
	WHERE Date >= DATEADD(week,-2,convert(datetime,GETDATE()))
	AND Date <= SYSUTCDATETIME();

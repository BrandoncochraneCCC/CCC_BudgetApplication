SELECT ISNULL(Resolved, 0) AS Resolved,ISNULL(InProgress, 0) AS 'In Progress',Username,Date,Description 
	FROM [BugReport]	
	WHERE Resolved = 1;
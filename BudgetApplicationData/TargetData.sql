CREATE TABLE [dbo].[TargetData]
(
	[TargetDataID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeTargetID] INT NOT NULL, 
	[NumStudents] DECIMAL(4,2)  NULL,
	[RevenueHours] INT NULL, 
	[Date] DATETIME NOT NULL, 
	PRIMARY KEY CLUSTERED ([TargetDataID] ASC),
	CONSTRAINT [FK_TargetData_EmployeeTarget_EmployeeTargetID] FOREIGN KEY ([EmployeeTargetID]) REFERENCES [EmployeeTarget]([EmployeeTargetID])
)

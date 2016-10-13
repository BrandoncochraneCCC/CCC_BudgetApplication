CREATE TABLE [dbo].[EmployeeTarget]
(
	[EmployeeTargetID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL, 
	[Hour] INT NOT NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([EmployeeTargetID] ASC),
	CONSTRAINT [FK_EmployeeTarget_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID])
)

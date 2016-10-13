CREATE TABLE [dbo].[ResidentTarget]
(
	[ResidentTargetID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL, 
	[Hour] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL
	PRIMARY KEY CLUSTERED ([ResidentTargetID] ASC),
	CONSTRAINT [FK_ResidentTarget_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID])
)

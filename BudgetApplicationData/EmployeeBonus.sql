CREATE TABLE [dbo].[EmployeeBonus]
(
	[EmployeeBonusID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL, 
	[Date] DATETIME NOT NULL, 
	[Value] DECIMAL(19,4) NOT NULL,
	PRIMARY KEY CLUSTERED ([EmployeeBonusID] ASC),
	CONSTRAINT [FK_Bonus_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID]) 
)

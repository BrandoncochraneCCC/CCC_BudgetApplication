CREATE TABLE [dbo].[Salary]
(
	[SalaryID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL,
	[PrevActual] DECIMAL(19,4) NULL, 
	[PrevBudget] DECIMAL(19,4) NULL, 
	[CurrentBudget] DECIMAL(19,4) NULL, 
	[HourlyRate] DECIMAL(19,4) NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([SalaryID] ASC),
	CONSTRAINT [FK_Salary_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID]),
)

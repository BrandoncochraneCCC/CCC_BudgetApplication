CREATE TABLE [dbo].[ServiceExpense]
(
	[ServiceExpenseID] INT NOT NULL IDENTITY(1, 1),
	[AccountNum] INT NULL, 
	[ParentID] INT NULL,
	[Name] NVARCHAR(50) NOT NULL, 
	[DepartmentID] INT NULL,
	PRIMARY KEY CLUSTERED ([ServiceExpenseID] ASC),
	CONSTRAINT [FK_ServiceExpense_Account_AccountNum] FOREIGN KEY ([AccountNum]) REFERENCES [Account]([AccountNum]),
	CONSTRAINT [FK_ServiceExpense_ServiceExpenseID] FOREIGN KEY ([ParentID]) REFERENCES [ServiceExpense]([ServiceExpenseID]),
	CONSTRAINT [FK_ServiceExpense_Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [Department]([DepartmentID])
)

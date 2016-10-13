CREATE TABLE [dbo].[ServiceExpenseComparison]
(
	[ServiceExpenseComparisonID] INT NOT NULL IDENTITy(1, 1),
	[ServiceExpenseID] INT NOT NULL,
	[PrevBudget] DECIMAL(19, 4) NULL, 
	[PrevActual] DECIMAL(19, 4) NULL,
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED([ServiceExpenseComparisonID] ASC),
	CONSTRAINT [ServiceExpenseComparison_ServiceExpense_ServiceExpenseID] FOREIGN KEY ([ServiceExpenseID]) REFERENCES [ServiceExpense]([ServiceExpenseID])
)


CREATE TABLE [dbo].[GAExpenseComparison]
(
	[GAExpenseComparisonID] INT NOT NULL IDENTITy(1, 1),
	[GAGroupID] INT NOT NULL,
	[PrevBudget] DECIMAL(19, 4) NULL, 
	[PrevActual] DECIMAL(19, 4) NULL,
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED([GAExpenseComparisonID] ASC),
	CONSTRAINT [GAExpenseComparison_GAGroup_GAGroupID] FOREIGN KEY ([GAGroupID]) REFERENCES [GAGroup]([GAGroupID])
)
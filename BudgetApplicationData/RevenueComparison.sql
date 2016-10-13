CREATE TABLE [dbo].[RevenueComparison]
(
	[RevenueComparisonID] INT NOT NULL IDENTITy(1, 1),
	[RevenueID] INT NOT NULL,
	[PrevBudget] DECIMAL(19, 4) NULL, 
	[PrevActual] DECIMAL(19, 4) NULL,
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([RevenueComparisonID] ASC),
	CONSTRAINT [RevenueComparison_Revenue_RevenueID] FOREIGN KEY ([RevenueID]) REFERENCES [Revenue]([RevenueID])
)

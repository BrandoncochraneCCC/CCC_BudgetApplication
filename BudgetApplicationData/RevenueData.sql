CREATE TABLE [dbo].[RevenueData]
(
	[RevenueDataID] INT NOT NULL IDENTITY(1, 1),
	[RevenueID] INT NOT NULL,
	[Value] DECIMAL(19,4) NOT NULL, 
	[Date] DATETIME NOT NULL
	PRIMARY KEY CLUSTERED ([RevenueDataID] ASC),
	CONSTRAINT [FK_dbo.Revenue_dbo.Revenue_RevenueID] FOREIGN KEY ([RevenueID]) REFERENCES [dbo].[Revenue] ([RevenueID])
)

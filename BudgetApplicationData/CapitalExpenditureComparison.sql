CREATE TABLE [dbo].[CapitalExpenditureComparison]
(
    [CapitalExpenditureComparisonID] INT             IDENTITY (1, 1) NOT NULL,
    [CapitalExpenditureID]           INT             NOT NULL,
    [PrevBudget]          DECIMAL (19, 4) NULL,
    [PrevActual]          DECIMAL (19, 4) NULL,
    [Year]                INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([CapitalExpenditureComparisonID] ASC),
    CONSTRAINT [FK_CapitalExpenditureComparison_CapitalExpenditure_CapitalExpenditureID] FOREIGN KEY ([CapitalExpenditureID]) REFERENCES [dbo].[CapitalExpenditure] ([CapitalExpenditureID])
	)

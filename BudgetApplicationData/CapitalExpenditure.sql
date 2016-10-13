CREATE TABLE [dbo].[CapitalExpenditure] (
    [CapitalExpenditureID] INT           IDENTITY (1, 1) NOT NULL,
    [ParentID]             INT           NULL,
    [PoolID]               INT           NULL,
    [Name]                 NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([CapitalExpenditureID] ASC),
    CONSTRAINT [FK_CapitalExpenditure_CapitalExpenditureID] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[CapitalExpenditure] ([CapitalExpenditureID]),
    CONSTRAINT [FK_CapitalExpenditure_Pool_PoolID] FOREIGN KEY ([PoolID]) REFERENCES [dbo].[Pool] ([PoolID])
);



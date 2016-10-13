CREATE TABLE [dbo].[CapitalExpenditureData]
(
    [CapitalExpenditureDataID] INT             IDENTITY (1, 1) NOT NULL,
    [CapitalExpenditureID]    INT             NOT NULL,
    [Value]         DECIMAL (19, 4) NOT NULL,
    [Date]          DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([CapitalExpenditureDataID] ASC),
    CONSTRAINT [FK_CapitalExpenditureData_CapitalExpenditureDataID] FOREIGN KEY ([CapitalExpenditureID]) REFERENCES [dbo].[CapitalExpenditure] ([CapitalExpenditureID])
)

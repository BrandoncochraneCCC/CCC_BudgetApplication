CREATE TABLE [dbo].[ServiceExpenseData] (
    [ServiceExpenseDataID] INT             IDENTITY (1, 1) NOT NULL,
    [ServiceExpenseID]  INT             NOT NULL,
    [Value]                DECIMAL (19, 4) NOT NULL,
    [Date]                 DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([ServiceExpenseDataID] ASC),
    CONSTRAINT [FK_ServiceExpenseData_ServiceExpense_ServiceExpenseID] FOREIGN KEY ([ServiceExpenseID]) REFERENCES [dbo].[ServiceExpense] ([ServiceExpenseID])
)
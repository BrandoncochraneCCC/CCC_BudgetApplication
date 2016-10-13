CREATE TABLE [dbo].[ContractHour] (
    [ContractHourID] INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeID]       INT             NOT NULL,
    [Hour]             DECIMAL (19, 4) NOT NULL,
    [Date]             DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([ContractHourID] ASC),
    CONSTRAINT [FK_ContractHour_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID])
);

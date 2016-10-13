CREATE TABLE [dbo].[EmployeeRaise] (
    [EmployeeRaiseID] INT             IDENTITY (1, 1) NOT NULL,
    [EmployeeID]      INT             NOT NULL,
    [Date]            DATETIME        NOT NULL,
    [Value]           DECIMAL (19, 4) NOT NULL,
    [isPercent]       BIT             NOT NULL,
    [OldSalary]       DECIMAL (19, 4) NULL,
    [NewSalary]       DECIMAL (19, 4) NULL,
    PRIMARY KEY CLUSTERED ([EmployeeRaiseID] ASC),
    CONSTRAINT [FK_Raise_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID])
);



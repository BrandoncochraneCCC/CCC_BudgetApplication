CREATE TABLE [dbo].[ProjectedFeePerHour] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Date]           DATETIME        NOT NULL,
    [EmployeeTypeID] INT             NOT NULL,
    [Value]          DECIMAL (19, 4) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AssumptionProjectedFeePerHour_EmployeeType_EmployeeTypeID] FOREIGN KEY ([EmployeeTypeID]) REFERENCES [dbo].[EmployeeType] ([EmployeeTypeID])
);


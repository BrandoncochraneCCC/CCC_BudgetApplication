CREATE TABLE [dbo].[InternTarget] (
    [InternTargetID] INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeID]       INT      NOT NULL,
    [Hour]             INT      NOT NULL,
    [Date]             DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([InternTargetID] ASC),
    CONSTRAINT [FK_InternTarget_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID])
);


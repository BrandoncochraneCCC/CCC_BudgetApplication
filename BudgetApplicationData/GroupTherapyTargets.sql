CREATE TABLE [dbo].[GroupTherapyTargets]
(
    [Id] INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeID]     INT      NOT NULL,
    [Hour]           INT      NOT NULL,
    [Date]           DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GroupTherapyTargets_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID])

)

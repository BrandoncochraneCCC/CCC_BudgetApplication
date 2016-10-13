CREATE TABLE [dbo].[Employee] (
    [EmployeeID]    INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentID]  INT           NULL,
    [TypeID]        INT           NULL,
    [FirstName]     NVARCHAR (50) NOT NULL,
    [LastName]      NVARCHAR (50) NOT NULL,
    [StartDate]     DATETIME      NULL,
    [EndDate]       DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([EmployeeID] ASC),
    CONSTRAINT [FK_Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [dbo].[Department] ([DepartmentID]),
    CONSTRAINT [FK_EmployeeType_TypeID] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[EmployeeType] ([EmployeeTypeID])
);


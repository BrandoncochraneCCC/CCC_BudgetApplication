CREATE TABLE [dbo].[EmployeeDeduction] (
    [EmployeeDeductionID] INT IDENTITY (1, 1) NOT NULL,
    [EmployeeID]         INT NOT NULL,
    [Year]               INT NOT NULL,
    [DeductionTypeID]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([EmployeeDeductionID] ASC),
    CONSTRAINT [FK_EmployeeBenefits_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeBenefits_DeductionType_DeductionTypeID] FOREIGN KEY ([DeductionTypeID]) REFERENCES [dbo].[DeductionType] ([DeductionTypeID])

);



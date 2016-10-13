CREATE TABLE [dbo].[AverageRate]
(
	[AverageRateID] INT NOT NULL IDENTITY(1, 1), 
	[EmployeeTypeID] INT NOT NULL, 
	[Value] DECIMAL(19,4) NOT NULL, 
	[Year] INT NOT NULL, 
	PRIMARY KEY CLUSTERED ([AverageRateID] ASC), 
	CONSTRAINT [FK_AverageRate_EmployeeType_EmployeeTypeID] FOREIGN KEY ([EmployeeTypeID]) REFERENCES [EmployeeType]([EmployeeTypeID])
)

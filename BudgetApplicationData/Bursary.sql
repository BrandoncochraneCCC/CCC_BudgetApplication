CREATE TABLE [dbo].[Bursary]
(
	[BursaryID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL,
	[BursaryValue] DECIMAL(19, 4) NOT NULL, 
	[Clawback] DECIMAL(19,4) NOT NULL, 
	[Date] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED ([BursaryID] ASC),
	CONSTRAINT [FK_Bursary_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID])
)

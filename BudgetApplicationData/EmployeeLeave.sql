CREATE TABLE [dbo].[EmployeeLeave]
(
	[EmployeeLeaveID] INT NOT NULL IDENTITY(1, 1),
	[EmployeeID] INT NOT NULL, 
	[StartDate] DATETIME NOT NULL, 
	[EndDate] DATETIME NULL, 
	[Description] NVARCHAR(MAX),
	PRIMARY KEY CLUSTERED ([EmployeeLeaveID] ASC),
	CONSTRAINT [FK_Leave_Employee_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employee]([EmployeeID]) 

)

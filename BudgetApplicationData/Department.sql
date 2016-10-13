CREATE TABLE [dbo].[Department]
(
	[DepartmentID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Description] NVARCHAR(50) NULL,
	PRIMARY KEY CLUSTERED ([DepartmentID] ASC)
)

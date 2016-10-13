CREATE TABLE [dbo].[Resident]
(
	[ResidentID] INT NOT NULL IDENTITY(1, 1),
	[FirstName] NVARCHAR(50) NOT NULL, 
	[LastName] NVARCHAR(50) NOT NULL, 
	[StartDate] DATETIME NULL, 
	[EndDate] DATETIME NULL,
	PRIMARY KEY ([ResidentID] ASC)
)

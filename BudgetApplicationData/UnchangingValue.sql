CREATE TABLE [dbo].[UnchangingValue]
(
	[UnchangingValueID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([UnchangingValueID] ASC)	
)

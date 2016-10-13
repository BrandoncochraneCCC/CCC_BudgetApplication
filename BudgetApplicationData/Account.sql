CREATE TABLE [dbo].[Account]
(
	[AccountNum] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL, 
	[PostingType] NVARCHAR(50) NULL, 
	[CategoryNum] NVARCHAR(50) NULL
)

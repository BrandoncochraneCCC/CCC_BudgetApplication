CREATE TABLE [dbo].[BugReport]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Username] NVARCHAR(50) NOT NULL, 
	[Date] DATETIME NOT NULL,
	[Description] NVARCHAR(300) NOT NULL, 
	[Resolved] BIT NULL, 
    [InProgress] BIT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
)

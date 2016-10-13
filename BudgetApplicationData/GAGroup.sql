CREATE TABLE [dbo].[GAGroup]
(
	[GAGroupID] INT NOT NULL IDENTITY(1, 1), 
	[ParentID] INT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[AccountNum] INT NULL, 
	PRIMARY KEY CLUSTERED ([GAGroupID]),
	CONSTRAINT [FK_GAGroup_Account_AccountNum] FOREIGN KEY ([AccountNum]) REFERENCES [Account]([AccountNum]),
	CONSTRAINT [FK_GAGroup_GAGroup_GroupID] FOREIGN KEY ([ParentID]) REFERENCES [GAGroup]([GAGroupID])
)

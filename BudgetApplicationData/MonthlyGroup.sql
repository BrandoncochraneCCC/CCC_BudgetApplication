CREATE TABLE [dbo].[MonthlyGroup]
(
	[MonthlyGroupID] INT NOT NULL IDENTITY(1, 1),
	[GroupID] INT NOT NULL,
	[Hours] INT NOT NULL, 
	[Date] DATETIME NOT NULL, 
	PRIMARY KEY CLUSTERED ([MonthlyGroupID] ASC),
	CONSTRAINT [FK_MonthlyGroup_Group_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [Group]([GroupID])
)

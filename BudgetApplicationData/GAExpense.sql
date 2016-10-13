CREATE TABLE [dbo].[GAExpense]
(
	[GAExpenseID] INT NOT NULL IDENTITY(1, 1), 
	[GroupID] INT NOT NULL, 
	[Value] DECIMAL(19,4) NOT NULL, 
	[Date] DATETIME NOT NULL, 
	[AccountNum] INT NULL, 
	PRIMARY KEY CLUSTERED ([GAExpenseID]),
	CONSTRAINT [FK_GAExpense_Group_GroupID] FOREIGN KEY ([GroupID]) REFERENCES [GAGroup]([GAGroupID]),
	CONSTRAINT [FK_GAExpense_Account_AccountNum] FOREIGN KEY ([AccountNum]) REFERENCES [Account]([AccountNum])
)

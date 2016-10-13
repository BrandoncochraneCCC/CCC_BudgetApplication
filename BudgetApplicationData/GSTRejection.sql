CREATE TABLE [dbo].[GSTRejection]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[GAGroupID] INT NULL,
	[ServiceExpenseID] INT NULL
	CONSTRAINT [GSTRejection_GAGroup_GAGroupID] FOREIGN KEY ([GAGroupID]) REFERENCES [GAGroup]([GAGroupID]),
	CONSTRAINT [GSTRejection_ServiceExpense_ServiceExpenseID] FOREIGN KEY ([ServiceExpenseID]) REFERENCES [ServiceExpense]([ServiceExpenseID])
)

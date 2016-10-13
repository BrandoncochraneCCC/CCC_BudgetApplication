CREATE TABLE [dbo].[Revenue]
(
	[RevenueID] INT NOT NULL IDENTITY (1, 1),
	[ParentID] INT NULL,
	[Name] NVARCHAR(50) NOT NULL, 
	PRIMARY KEY CLUSTERED ([RevenueID] ASC), 
    CONSTRAINT [FK_Revenue_RevenueID] FOREIGN KEY ([ParentID]) REFERENCES [Revenue]([RevenueID]) 

)

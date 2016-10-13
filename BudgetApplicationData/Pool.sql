CREATE TABLE [dbo].[Pool] (
    [PoolID]           INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)   NOT NULL,
    [AccountNum]       INT             NULL,
    [DepreciationRate] DECIMAL (19, 4) NOT NULL,
    [StraightLine]     BIT             NULL,
	[_isDeferred] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([PoolID] ASC),
    CONSTRAINT [FK_Pool_Account_AccountNum] FOREIGN KEY ([AccountNum]) REFERENCES [dbo].[Account] ([AccountNum])
);



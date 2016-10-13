CREATE TABLE [dbo].[DeferredAmortization]
(
	[DeferredAmortizationID] INT NOT NULL IDENTITY(1, 1), 
	[PoolID] INT NOT NULL, 
	[PoolBalance] DECIMAL(19, 4) NOT NULL, 
	[AccumulatedAmortization] DECIMAL (19, 4) NOT NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([DeferredAmortizationID]),
	CONSTRAINT [FK_DeferredAmortization_Pool_PoolID] FOREIGN KEY ([PoolID]) REFERENCES [Pool]([PoolID])
)

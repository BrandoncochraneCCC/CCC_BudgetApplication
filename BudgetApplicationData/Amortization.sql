CREATE TABLE [dbo].[Amortization]
(
	[AmortizationID] INT NOT NULL IDENTITY(1, 1), 
	[PoolID] INT NOT NULL, 
	[PoolBalance] DECIMAL(19, 4) NOT NULL, 
	[AccumulatedAmortization] DECIMAL (19, 4) NOT NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([AmortizationID]),
	CONSTRAINT [FK_Amortization_Pool_PoolID] FOREIGN KEY ([PoolID]) REFERENCES [Pool]([PoolID])
)

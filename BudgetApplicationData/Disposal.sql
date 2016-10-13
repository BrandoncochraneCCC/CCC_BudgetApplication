CREATE TABLE [dbo].[Disposal] (
    [DisposalID]             INT             IDENTITY (1, 1) NOT NULL,
    [AmortizationID]         INT             NULL,
    [DeferredAmortizationID] INT             NULL,
    [Name]                   NVARCHAR (50)   NOT NULL,
    [Value]                  DECIMAL (19, 4) NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([DisposalID] ASC),
    CONSTRAINT [FK_Disposal_Amortization_AmortizationID] FOREIGN KEY ([AmortizationID]) REFERENCES [dbo].[Amortization] ([AmortizationID]),
    CONSTRAINT [FK_Disposal_DeferredAmortization_DeferredAmortizationID] FOREIGN KEY ([DeferredAmortizationID]) REFERENCES [dbo].[DeferredAmortization] ([DeferredAmortizationID])
);



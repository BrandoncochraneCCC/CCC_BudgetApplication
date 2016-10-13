CREATE TABLE [dbo].[Addition] (
    [AdditionID]             INT             IDENTITY (1, 1) NOT NULL,
    [AmortizationID]         INT             NULL,
    [Name]                   NVARCHAR (50)   NOT NULL,
    [Value]                  DECIMAL (19, 4) NOT NULL,
    [Date]                   DATETIME        NOT NULL,
    [DeferredAmortizationID] INT             NULL,
    PRIMARY KEY CLUSTERED ([AdditionID] ASC),
    CONSTRAINT [FK_Addition_Amortization_AmortizationID] FOREIGN KEY ([AmortizationID]) REFERENCES [dbo].[Amortization] ([AmortizationID]),
    CONSTRAINT [FK_Addition_DeferredAmortization_DeferredAmortizationID] FOREIGN KEY ([DeferredAmortizationID]) REFERENCES [dbo].[DeferredAmortization] ([DeferredAmortizationID])
);



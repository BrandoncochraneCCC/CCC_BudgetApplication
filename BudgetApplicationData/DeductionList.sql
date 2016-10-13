CREATE TABLE [dbo].[DeductionList]
(
	[DeductionListID] INT NOT NULL IDENTITY(1, 1),
	[DeductionTypeID] INT NOT NULL, 
	[Year] INT NOT NULL, 
	[Max] DECIMAL(19,4) NULL, 
	[Rate] DECIMAL(19, 4) NULL,
	PRIMARY KEY CLUSTERED ([DeductionListID] ASC),
	CONSTRAINT [FK_DeductionList_DeductionType_DeductionTypeID] FOREIGN KEY ([DeductionTypeID]) REFERENCES [DeductionType]([DeductionTypeID])
)

CREATE TABLE [dbo].[ProgramSection]
(
	[ProgramSectionID] INT NOT NULL IDENTITY(1, 1),
	[ProgramID] INT NOT NULL, 
	[Fee] DECIMAL(19,4) NOT NULL, 
	[NumFacilitator] INT NOT NULL, 
	[Year] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([ProgramSectionID] ASC),
	CONSTRAINT [FK_programSection_Program_ProgramID] FOREIGN KEY ([ProgramID]) REFERENCES [Program]([ProgramID]),
)

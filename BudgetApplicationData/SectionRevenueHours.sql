CREATE TABLE [dbo].[SectionRevenueHours]
(
	[SectionRevenueHoursID] INT NOT NULL IDENTITY(1, 1),
	[ProgramSectionID] INT NOT NULL, 
	[PercentRevenueHours] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED ([SectionRevenueHoursID] ASC),
	CONSTRAINT [FK_SectionRevenueHours_ProgramSection_ProgramSectionID] FOREIGN KEY ([ProgramSectionID]) REFERENCES [ProgramSection]([ProgramSectionID])
)

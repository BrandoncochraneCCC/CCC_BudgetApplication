CREATE TABLE [dbo].[Group]
(
	[GroupID] INT NOT NULL IDENTITY(1, 1),
	[ProgramSectionID] INT NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL, 
	[NumClients] INT NOT NULL, 
	PRIMARY KEY CLUSTERED ([GroupID] ASC),
	CONSTRAINT [FK_Group_ProgramSection_ProgramSectionID] FOREIGN KEY ([ProgramSectionID]) REFERENCES [ProgramSection]([ProgramSectionID])
)

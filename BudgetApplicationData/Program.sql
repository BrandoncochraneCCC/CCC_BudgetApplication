CREATE TABLE [dbo].[Program]
(
	[ProgramID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Acronym] NVARCHAR(5) NULL,
	[CounsellingGroupTypeID] INT NULL, 
	PRIMARY KEY CLUSTERED ([ProgramID] ASC),
	CONSTRAINT [Program_CounsellingGroupType_Id] FOREIGN KEY ([CounsellingGroupTypeID]) REFERENCES [CounsellingGroupType]([Id])
)

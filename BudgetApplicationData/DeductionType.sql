CREATE TABLE [dbo].[DeductionType]
(
	[DeductionTypeID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL,
	[Acronym] NVARCHAR(15) NULL,
	PRIMARY KEY CLUSTERED ([DeductionTypeID])
)

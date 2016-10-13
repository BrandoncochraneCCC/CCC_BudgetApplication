CREATE TABLE [dbo].[CounsellingService]
(
	[CounsellingServiceID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL,
	PRIMARY KEY CLUSTERED([CounsellingServiceID] ASC)
)

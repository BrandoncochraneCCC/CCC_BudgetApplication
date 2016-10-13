CREATE TABLE [dbo].[CounsellingServiceData]
(
	[CounsellingServiceDataID] INT NOT NULL IDENTITY(1, 1),
	[CounsellingServiceID] INT NOT NULL,
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL, 
	PRIMARY KEY CLUSTERED([CounsellingServiceDataID] ASC),
	CONSTRAINT [FK_CounsellingServiceData_CounsellingService_CounsellingServiceID] FOREIGN KEY ([CounsellingServiceID]) REFERENCES [CounsellingService]([CounsellingServiceID])
)

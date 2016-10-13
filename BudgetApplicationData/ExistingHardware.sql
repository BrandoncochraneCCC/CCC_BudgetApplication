CREATE TABLE [dbo].[ExistingHardware]
(
	[ExistingHardwareID] INT NOT NULL IDENTITY(1, 1), 
	[GAGroupID] INT NOT NULL, 
	[Name] NVARCHAR(50) NOT NULL, 
	[Age] INT NULL, 
	[Amortization] DECIMAL(19,4) NULL, 
	PRIMARY KEY CLUSTERED ([ExistingHardwareID] ASC),
	CONSTRAINT [FK_ExistingHardware_GAGroup_GAGroupID] FOREIGN KEY ([GAGroupID]) REFERENCES [GAGroup]([GAGroupID])
)

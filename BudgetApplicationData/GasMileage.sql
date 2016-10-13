CREATE TABLE [dbo].[GasMileage]
(
	[GasMileageID] INT NOT NULL IDENTITY(1, 1),
	[ConferenceID] INT NOT NULL,
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL, 
	PRIMARY KEY CLUSTERED([GasMileageID] ASC),
	CONSTRAINT [FK_GasMileage_Conference_ConferenceID] FOREIGN KEY([ConferenceID]) REFERENCES [Conference]([ConferenceID])
)

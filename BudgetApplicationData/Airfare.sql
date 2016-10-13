CREATE TABLE [dbo].[Airfare]
(
	[AirfareID] INT NOT NULL IDENTITY(1, 1),
	[ConferenceID] INT NOT NULL,
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL, 
	PRIMARY KEY CLUSTERED([AirfareID] ASC),
	CONSTRAINT [FK_Airfare_Conference_ConferenceID] FOREIGN KEY([ConferenceID]) REFERENCES [Conference]([ConferenceID])
)

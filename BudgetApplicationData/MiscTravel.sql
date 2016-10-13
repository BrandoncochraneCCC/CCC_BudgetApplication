CREATE TABLE [dbo].[MiscTravel]
(
	[MiscTravelID] INT NOT NULL IDENTITY(1, 1), 
	[ConferenceID] INT NOT NULL, 
	[Value] DECIMAL(19, 4) NOT NULL,
	[Date] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED ([MiscTravelID] ASC),
	CONSTRAINT [FK_MiscTravel_Conference_ConferenceID] FOREIGN KEY ([ConferenceID]) REFERENCES [Conference]([ConferenceID])
)

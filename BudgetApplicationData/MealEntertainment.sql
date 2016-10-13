CREATE TABLE [dbo].[MealEntertainment]
(
	[MealEntertainmentID] INT NOT NULL IDENTITY(1, 1),
	[ConferenceID] INT NOT NULL, 
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED([MealEntertainmentID] ASC),
	CONSTRAINT [FK_MealEntertainment_Conference_ConferenceID] FOREIGN KEY ([ConferenceID]) REFERENCES [Conference]([ConferenceID])
)

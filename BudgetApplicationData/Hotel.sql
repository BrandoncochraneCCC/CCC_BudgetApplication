CREATE TABLE [dbo].[Hotel]
(
	[HotelID] INT NOT NULL IDENTITY(1,1),
	[ConferenceID] INT NOT NULL, 
	[Value] DECIMAL(19, 4) NOT NULL, 
	[Date] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED ([HotelID] ASC),
	CONSTRAINT [FK_Hotel_Conference_ConferenceID] FOREIGN KEY([ConferenceID]) REFERENCES [Conference]([ConferenceID])

)

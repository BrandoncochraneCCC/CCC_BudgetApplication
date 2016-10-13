CREATE TABLE [dbo].[NonRevenueHour]
(
	[NonRevenueHourID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL, 
	PRIMARY KEY CLUSTERED([NonRevenueHourID] ASC)
)

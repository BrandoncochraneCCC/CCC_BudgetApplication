CREATE TABLE [dbo].[NonRevenueHourData]
(
	[NonRevenueHourDataID] INT NOT NULL IDENTITY(1, 1),
	[TargetDataID] INT NOT NULL,
	[NonRevenueHourID] INT NOT NULL,
	[Value] INT NOT NULL, 
	PRIMARY KEY CLUSTERED([NonRevenueHourDataID] ASC),
	CONSTRAINT [FK_NonRevenueHourData_NonRevenueHour_NonRevenueHourID] FOREIGN KEY ([NonRevenueHourID]) REFERENCES [NonRevenueHour]([NonRevenueHourID]),
	CONSTRAINT [FK_NonRevenueData_TargetData_TargetDataID] FOREIGN KEY([TargetDataID]) REFERENCES [TargetData]([TargetDataID])
)

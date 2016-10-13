CREATE TABLE [dbo].[ActualData]
(
		[ActualDataID] INT NOT NULL IDENTITY(1, 1),
		[ActualValueID] INT NOT NULL, 
		[Value] DECIMAL(19, 4) NOT NULL, 
		[Date] DATETIME NOT NULL, 
		PRIMARY KEY CLUSTERED ([ActualDataID] ASC), 
		CONSTRAINT [FK_ActualData_ActualValue_ActualValueID] FOREIGN KEY ([ActualValueID]) REFERENCES [ActualValue]([ActualValueID])
)

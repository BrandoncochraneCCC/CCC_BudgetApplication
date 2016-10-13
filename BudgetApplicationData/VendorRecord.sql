CREATE TABLE [dbo].[VendorRecord]
(
	[VendorRecordID] INT NOT NULL IDENTITY(1, 1), 
	[VendorID] INT NOT NULL, 
	[YearToDate] DECIMAL(19,4) NULL, 
	[Year] INT NOT NULL, 
	PRIMARY KEY CLUSTERED ([VendorRecordID] ASC)
)

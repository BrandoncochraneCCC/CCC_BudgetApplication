﻿CREATE TABLE [dbo].[CompanyGasMileage]
(
	[CompanyGasMileageID] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL, 
	PRIMARY KEY CLUSTERED([CompanyGasMileageID] ASC)
)

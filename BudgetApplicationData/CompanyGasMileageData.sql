CREATE TABLE [dbo].[CompanyGasMileageData] (
    [CompanyGasMileageDataID] INT             IDENTITY (1, 1) NOT NULL,
    [CompanyGasMileageID] INT NOT NULL,
	[Value]                   DECIMAL (19, 4) NOT NULL,
    [DATE]                    DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([CompanyGasMileageDataID] ASC),
	CONSTRAINT [FK_CompanyGasMileageData_CompanygasMileage_CompanyGasMileageID] FOREIGN KEY ([CompanyGasMileageID]) REFERENCES [CompanyGasMileage]([CompanyGasMileageID])
)

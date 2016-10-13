CREATE TABLE [dbo].[BenefitData] (
    [BenefitDataID] INT             IDENTITY (1, 1) NOT NULL,
    [BenefitPlanID] INT             NOT NULL,
    [Value]         DECIMAL (19, 4) NOT NULL,
    [Year]          INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([BenefitDataID] ASC)
);



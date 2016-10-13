CREATE TABLE [dbo].[UserBuiltSummaryData] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [SummaryID]   INT           NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Table]       NVARCHAR (50) NOT NULL,
    [TableItemID] INT           NOT NULL,
    [CategoryID]  INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Category_Catergory_Id] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([Id]),
    CONSTRAINT [FK_UserBuiltSummaryData_UserBuiltSummary_Id] FOREIGN KEY ([SummaryID]) REFERENCES [dbo].[UserBuiltSummary] ([Id])
);


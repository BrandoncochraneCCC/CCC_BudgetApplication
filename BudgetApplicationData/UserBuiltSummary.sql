CREATE TABLE [dbo].[UserBuiltSummary]
(
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
	[ParentID] INT NULL, 
    [Name]       NVARCHAR (50)   NOT NULL,
    [Desc]       NVARCHAR (140)  NULL,
    [Year]       INT             NULL,
    [Percentage] DECIMAL (19, 4) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserBuiltSummary_UserBuiltSummary_Id] FOREIGN KEY ([ParentID]) REFERENCES [UserBuiltSummary]([Id])
)

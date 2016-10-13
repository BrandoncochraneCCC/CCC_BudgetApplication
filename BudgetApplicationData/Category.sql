CREATE TABLE [dbo].[Category] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ParentID]    INT            NULL,
    [Name]        NVARCHAR (40)  NOT NULL,
    [Description] NVARCHAR (140) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Category_Category_Id] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[Category] ([Id])
);



CREATE TABLE [dbo].[Conference] (
    [ConferenceID]   INT           IDENTITY (1, 1) NOT NULL,
    [ConferenceName] NVARCHAR (50) NOT NULL,
    [AttendeeName]   NVARCHAR (50) NOT NULL,
    [GAGroupID]      INT           NOT NULL,
    [Year]           INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ConferenceID] ASC),
    CONSTRAINT [FK_Conference_GAGroup_GAGroupID] FOREIGN KEY ([GAGroupID]) REFERENCES [dbo].[GAGroup] ([GAGroupID])
);



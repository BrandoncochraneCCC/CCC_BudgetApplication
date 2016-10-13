CREATE TABLE [dbo].[Log] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [logDate]    DATETIME       NULL,
    [logThread]  VARCHAR (50)   NULL,
    [logLevel]   VARCHAR (50)   NULL,
    [logSource]  VARCHAR (300)  NULL,
    [logMessage] VARCHAR (4000) NULL,
    [exception]  VARCHAR (4000) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


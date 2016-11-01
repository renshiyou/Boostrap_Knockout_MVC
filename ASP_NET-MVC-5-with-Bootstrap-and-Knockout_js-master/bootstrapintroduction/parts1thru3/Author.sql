CREATE TABLE [dbo].[Author] ( 
    [Id] INT IDENTITY (1, 1) NOT NULL, 
    [FirstName] NVARCHAR (200) NULL, 
    [LastName]  NVARCHAR (200) NULL, 
	[Biography]  NVARCHAR (2000) NULL, 
    CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED ([Id] ASC) 
); 
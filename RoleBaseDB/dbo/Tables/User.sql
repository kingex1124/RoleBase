CREATE TABLE [dbo].[User] (
    [UserID]      INT            IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (MAX) NOT NULL,
    [Password]    NVARCHAR (MAX) NOT NULL,
    [Phone]       NVARCHAR (MAX) NULL,
    [Email]       NVARCHAR (MAX) NULL,
    [AccountName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);




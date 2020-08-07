CREATE TABLE [dbo].[RoleUser] (
    [RoleID] INT NOT NULL,
    [UserID] INT NOT NULL,
    CONSTRAINT [PK_RoleUser] PRIMARY KEY CLUSTERED ([RoleID] ASC, [UserID] ASC)
);




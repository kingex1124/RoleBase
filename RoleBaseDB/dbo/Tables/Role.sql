﻿CREATE TABLE [dbo].[Role] (
    [RoleID]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);




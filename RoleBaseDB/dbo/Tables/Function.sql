﻿CREATE TABLE [dbo].[Function] (
    [FunctionID]  INT            IDENTITY (1, 1) NOT NULL,
    [Url]         NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsMenu]      BIT            NULL,
    [Parent]      INT            NULL,
    [Title]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Function] PRIMARY KEY CLUSTERED ([FunctionID] ASC)
);






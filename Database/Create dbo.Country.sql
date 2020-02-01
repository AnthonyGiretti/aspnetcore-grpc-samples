USE [DemoOrmLite]
GO

/****** Object: Table [dbo].[Country] Script Date: 1/31/2020 11:38:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Country] (
    [CountryId]   INT          IDENTITY (1, 1) NOT NULL,
    [CountryName] VARCHAR (50) NOT NULL,
    [Description] VARCHAR (50) NULL
);



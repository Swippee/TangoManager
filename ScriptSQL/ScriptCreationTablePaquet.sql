USE [TangoManager]
GO

/****** Object:  Table [dbo].[Paquet]    Script Date: 09/02/2023 22:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Paquet](
	[Nom] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Score] [int] NULL,
	[DateCreation] [datetime] NOT NULL,
	[DateDernierQuiz] [datetime] NULL,
 CONSTRAINT [PK__Paquet] PRIMARY KEY CLUSTERED 
(
	[Nom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



USE [TangoManager]
GO

/****** Object:  Table [dbo].[Carte]    Script Date: 09/02/2023 21:59:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Carte](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaquetNom] [nvarchar](20) NULL,
	[Question] [nvarchar](50) NOT NULL,
	[Reponse] [nvarchar](50) NOT NULL,
	[Score] [nchar](10) NOT NULL,
	[DateCreation] [datetime] NULL,
	[DateDernierQuiz] [datetime] NOT NULL,
 CONSTRAINT [PK_Carte] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Carte]  WITH CHECK ADD  CONSTRAINT [FK__Carte__PaquetNom] FOREIGN KEY([PaquetNom])
REFERENCES [dbo].[Paquet] ([Nom])
GO

ALTER TABLE [dbo].[Carte] CHECK CONSTRAINT [FK__Carte__PaquetNom]
GO



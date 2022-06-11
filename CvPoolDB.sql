USE [CvPoolDB]
GO
/****** Object:  Table [dbo].[Certificates]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Certificates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[CertificateNo] [nvarchar](50) NOT NULL,
	[Duration] [int] NOT NULL,
	[Information] [text] NOT NULL,
 CONSTRAINT [PK_Certificates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Industry] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Logo] [nvarchar](max) NOT NULL,
	[Information] [text] NOT NULL,
	[Number] [nvarchar](50) NULL,
	[Password] [nvarchar](24) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Competencies]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Competencies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Competence] [nvarchar](100) NOT NULL,
	[Rate] [int] NOT NULL,
 CONSTRAINT [PK_Competencies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[CourseName] [nvarchar](200) NOT NULL,
	[CourseCompany] [nvarchar](200) NOT NULL,
	[Information] [text] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Educations]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Educations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[DiplomaName] [nvarchar](200) NOT NULL,
	[SchoolName] [nvarchar](200) NOT NULL,
	[SectionName] [nvarchar](200) NOT NULL,
	[Information] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Foreign Languages]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Foreign Languages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Language] [nvarchar](50) NOT NULL,
	[Rate] [int] NOT NULL,
 CONSTRAINT [PK_Foreign Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hobbies]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hobbies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Hobby] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personal Informations]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personal Informations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Surname] [nvarchar](30) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[PlaceOfBirth] [nvarchar](100) NOT NULL,
	[Gender] [nvarchar](5) NOT NULL,
	[Nation] [nvarchar](50) NOT NULL,
	[MaritalStatus] [nvarchar](30) NOT NULL,
	[DriverLicense] [nvarchar](3) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Picture] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Date] [date] NOT NULL,
	[Information] [text] NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[References]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[References](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Surname] [nvarchar](30) NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_References] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Work Experiences]    Script Date: 10.04.2022 23:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Work Experiences](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[CompanyLocation] [nvarchar](100) NOT NULL,
	[Information] [text] NOT NULL,
 CONSTRAINT [PK_Work Experiences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Companies] ON 

INSERT [dbo].[Companies] ([Id], [Name], [Title], [Industry], [PhoneNumber], [Email], [Logo], [Information], [Number], [Password]) VALUES (2, N'Greamer Games', N'Make it, Dream it, Gream it!', N'Game Development', N'+905465389841', N'greamergames@gmail.com', N'GreamerGames.png', N'2D Game Development Company', N'423956629141', N'cvpoolbedron555')
SET IDENTITY_INSERT [dbo].[Companies] OFF
SET IDENTITY_INSERT [dbo].[Personal Informations] ON 

INSERT [dbo].[Personal Informations] ([Id], [Name], [Surname], [DateOfBirth], [PlaceOfBirth], [Gender], [Nation], [MaritalStatus], [DriverLicense], [Address], [PhoneNumber], [Email], [Picture]) VALUES (9, N'Gökay', N'Ürenç', CAST(N'1999-12-24' AS Date), N'Adana Seyhan', N'Bay', N'TC', N'Bekar', N'Yok', N'İstanbul Üsküdar', N'+905465389841', N'greamergames@gmail.com', N'Picture.jpg')
SET IDENTITY_INSERT [dbo].[Personal Informations] OFF
ALTER TABLE [dbo].[Certificates]  WITH CHECK ADD  CONSTRAINT [FK_Certificates_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Certificates] CHECK CONSTRAINT [FK_Certificates_Personal Informations]
GO
ALTER TABLE [dbo].[Competencies]  WITH CHECK ADD  CONSTRAINT [FK_Competencies_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Competencies] CHECK CONSTRAINT [FK_Competencies_Personal Informations]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Personal Informations]
GO
ALTER TABLE [dbo].[Educations]  WITH CHECK ADD  CONSTRAINT [FK_Educations_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Educations] CHECK CONSTRAINT [FK_Educations_Personal Informations]
GO
ALTER TABLE [dbo].[Foreign Languages]  WITH CHECK ADD  CONSTRAINT [FK_Foreign Languages_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Foreign Languages] CHECK CONSTRAINT [FK_Foreign Languages_Personal Informations]
GO
ALTER TABLE [dbo].[Hobbies]  WITH CHECK ADD  CONSTRAINT [FK_Hobbies_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Hobbies] CHECK CONSTRAINT [FK_Hobbies_Personal Informations]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Personal Informations]
GO
ALTER TABLE [dbo].[References]  WITH CHECK ADD  CONSTRAINT [FK_References_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[References] CHECK CONSTRAINT [FK_References_Personal Informations]
GO
ALTER TABLE [dbo].[Work Experiences]  WITH CHECK ADD  CONSTRAINT [FK_Work Experiences_Personal Informations] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Personal Informations] ([Id])
GO
ALTER TABLE [dbo].[Work Experiences] CHECK CONSTRAINT [FK_Work Experiences_Personal Informations]
GO

USE [master]
GO
/****** Object:  Database [Questionnaire]    Script Date: 2022/4/29 下午 07:31:28 ******/
CREATE DATABASE [Questionnaire]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Questionnaire', FILENAME = N'D:\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Questionnaire.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Questionnaire_log', FILENAME = N'D:\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Questionnaire_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Questionnaire] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Questionnaire].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Questionnaire] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Questionnaire] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Questionnaire] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Questionnaire] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Questionnaire] SET ARITHABORT OFF 
GO
ALTER DATABASE [Questionnaire] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Questionnaire] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Questionnaire] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Questionnaire] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Questionnaire] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Questionnaire] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Questionnaire] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Questionnaire] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Questionnaire] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Questionnaire] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Questionnaire] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Questionnaire] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Questionnaire] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Questionnaire] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Questionnaire] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Questionnaire] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Questionnaire] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Questionnaire] SET RECOVERY FULL 
GO
ALTER DATABASE [Questionnaire] SET  MULTI_USER 
GO
ALTER DATABASE [Questionnaire] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Questionnaire] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Questionnaire] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Questionnaire] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Questionnaire] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Questionnaire] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Questionnaire', N'ON'
GO
ALTER DATABASE [Questionnaire] SET QUERY_STORE = OFF
GO
USE [Questionnaire]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [uniqueidentifier] NOT NULL,
	[Account] [varchar](50) NOT NULL,
	[PWD] [varchar](50) NOT NULL,
	[UserLevel] [int] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountCheck]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountCheck](
	[CheckID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [uniqueidentifier] NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[Checks] [bit] NOT NULL,
 CONSTRAINT [PK_AccountCheck_1] PRIMARY KEY CLUSTERED 
(
	[CheckID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Content]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[TitleID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsEnable] [bit] NOT NULL,
 CONSTRAINT [PK_Content_1] PRIMARY KEY CLUSTERED 
(
	[QuestionnaireID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CQ]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CQ](
	[CQID] [int] IDENTITY(1,1) NOT NULL,
	[CQTitle] [nvarchar](200) NOT NULL,
	[QuesTypeID] [int] NOT NULL,
	[CQChoice] [nvarchar](max) NULL,
	[Necessary] [bit] NOT NULL,
 CONSTRAINT [PK_CQ] PRIMARY KEY CLUSTERED 
(
	[CQID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuesDetail]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuesDetail](
	[QuesID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[TitleID] [int] NOT NULL,
	[QuesTitle] [nvarchar](200) NOT NULL,
	[QuesChoice] [nvarchar](max) NULL,
	[QuesTypeID] [int] NOT NULL,
	[Necessary] [bit] NOT NULL,
	[Count] [int] NULL,
 CONSTRAINT [PK_QuesDetail] PRIMARY KEY CLUSTERED 
(
	[QuesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuesType]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuesType](
	[QuesTypeID] [int] NOT NULL,
	[QuesType] [nchar](10) NOT NULL,
 CONSTRAINT [PK_QuesType] PRIMARY KEY CLUSTERED 
(
	[QuesTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Statistic]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statistic](
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[QuesID] [int] NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[AnsCount] [int] NULL,
 CONSTRAINT [PK_Statistic_1] PRIMARY KEY CLUSTERED 
(
	[QuestionnaireID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfos]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfos](
	[UserID] [uniqueidentifier] NOT NULL,
	[AccountID] [uniqueidentifier] NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[CreateDate] [date] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Age] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserInfos] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserQuesDetails]    Script Date: 2022/4/29 下午 07:31:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserQuesDetails](
	[AnsID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[QuesID] [int] NOT NULL,
	[Answer] [nvarchar](max) NOT NULL,
	[AccountID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UserQuesDetails] PRIMARY KEY CLUSTERED 
(
	[AnsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_ID]  DEFAULT (newid()) FOR [AccountID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_IsEnable]  DEFAULT ('TRUE') FOR [IsEnable]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[AccountCheck] ADD  CONSTRAINT [DF_AccountCheck_Check]  DEFAULT ('FALSE') FOR [Checks]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_QuesID]  DEFAULT (newid()) FOR [QuestionnaireID]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_IsEnable]  DEFAULT ('TRUE') FOR [IsEnable]
GO
ALTER TABLE [dbo].[CQ] ADD  CONSTRAINT [DF_CQ_IsEnable]  DEFAULT ('TRUE') FOR [Necessary]
GO
ALTER TABLE [dbo].[QuesDetail] ADD  CONSTRAINT [DF_QuesDetail_IsEnable]  DEFAULT ('TRUE') FOR [Necessary]
GO
ALTER TABLE [dbo].[UserInfos] ADD  CONSTRAINT [DF_UserInfos_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[AccountCheck]  WITH CHECK ADD  CONSTRAINT [FK_AccountCheck_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[AccountCheck] CHECK CONSTRAINT [FK_AccountCheck_Account]
GO
ALTER TABLE [dbo].[CQ]  WITH CHECK ADD  CONSTRAINT [FK_CQ_QuesType] FOREIGN KEY([QuesTypeID])
REFERENCES [dbo].[QuesType] ([QuesTypeID])
GO
ALTER TABLE [dbo].[CQ] CHECK CONSTRAINT [FK_CQ_QuesType]
GO
ALTER TABLE [dbo].[QuesDetail]  WITH CHECK ADD  CONSTRAINT [FK_QuesDetail_Content] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Content] ([QuestionnaireID])
GO
ALTER TABLE [dbo].[QuesDetail] CHECK CONSTRAINT [FK_QuesDetail_Content]
GO
ALTER TABLE [dbo].[QuesDetail]  WITH CHECK ADD  CONSTRAINT [FK_QuesDetail_QuesType] FOREIGN KEY([QuesTypeID])
REFERENCES [dbo].[QuesType] ([QuesTypeID])
GO
ALTER TABLE [dbo].[QuesDetail] CHECK CONSTRAINT [FK_QuesDetail_QuesType]
GO
ALTER TABLE [dbo].[Statistic]  WITH CHECK ADD  CONSTRAINT [FK_Statistic_QuesDetail] FOREIGN KEY([QuesID])
REFERENCES [dbo].[QuesDetail] ([QuesID])
GO
ALTER TABLE [dbo].[Statistic] CHECK CONSTRAINT [FK_Statistic_QuesDetail]
GO
ALTER TABLE [dbo].[UserInfos]  WITH CHECK ADD  CONSTRAINT [FK_UserInfos_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([AccountID])
GO
ALTER TABLE [dbo].[UserInfos] CHECK CONSTRAINT [FK_UserInfos_Account]
GO
ALTER TABLE [dbo].[UserInfos]  WITH CHECK ADD  CONSTRAINT [FK_UserInfos_Content] FOREIGN KEY([QuestionnaireID])
REFERENCES [dbo].[Content] ([QuestionnaireID])
GO
ALTER TABLE [dbo].[UserInfos] CHECK CONSTRAINT [FK_UserInfos_Content]
GO
ALTER TABLE [dbo].[UserQuesDetails]  WITH CHECK ADD  CONSTRAINT [FK_UserQuesDetails_QuesDetail] FOREIGN KEY([QuesID])
REFERENCES [dbo].[QuesDetail] ([QuesID])
GO
ALTER TABLE [dbo].[UserQuesDetails] CHECK CONSTRAINT [FK_UserQuesDetails_QuesDetail]
GO
ALTER TABLE [dbo].[UserQuesDetails]  WITH CHECK ADD  CONSTRAINT [FK_UserQuesDetails_UserInfos1] FOREIGN KEY([UserID])
REFERENCES [dbo].[UserInfos] ([UserID])
GO
ALTER TABLE [dbo].[UserQuesDetails] CHECK CONSTRAINT [FK_UserQuesDetails_UserInfos1]
GO
USE [master]
GO
ALTER DATABASE [Questionnaire] SET  READ_WRITE 
GO

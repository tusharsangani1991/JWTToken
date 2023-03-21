USE [master]
GO
/****** Object:  Database [JWTDemo]    Script Date: 2023-03-21 03:44:45 PM ******/
CREATE DATABASE [JWTDemo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JWTDemo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JWTDemo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JWTDemo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\JWTDemo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JWTDemo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JWTDemo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JWTDemo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JWTDemo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JWTDemo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JWTDemo] SET ARITHABORT OFF 
GO
ALTER DATABASE [JWTDemo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JWTDemo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JWTDemo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JWTDemo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JWTDemo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JWTDemo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JWTDemo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JWTDemo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JWTDemo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JWTDemo] SET  ENABLE_BROKER 
GO
ALTER DATABASE [JWTDemo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JWTDemo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JWTDemo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JWTDemo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JWTDemo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JWTDemo] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [JWTDemo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JWTDemo] SET RECOVERY FULL 
GO
ALTER DATABASE [JWTDemo] SET  MULTI_USER 
GO
ALTER DATABASE [JWTDemo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JWTDemo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JWTDemo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JWTDemo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JWTDemo] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'JWTDemo', N'ON'
GO
ALTER DATABASE [JWTDemo] SET QUERY_STORE = OFF
GO
USE [JWTDemo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiTokens]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[AccessToken] [nvarchar](max) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshExpiryTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ApiTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupRole]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupRole](
	[Id] [uniqueidentifier] NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[Inherited] [bit] NOT NULL,
 CONSTRAINT [PK_GroupRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[Module] [nvarchar](max) NULL,
	[Add] [bit] NOT NULL,
	[Edit] [bit] NOT NULL,
	[View] [bit] NOT NULL,
	[Delete] [bit] NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Role] [nvarchar](max) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2023-03-21 03:44:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230321035953_InitialChanges', N'6.0.6')
GO
INSERT [dbo].[Group] ([Id], [Name]) VALUES (N'2eb7e23e-5e2a-4fe8-999f-054b04299632', N'SuperAdmin')
GO
INSERT [dbo].[Group] ([Id], [Name]) VALUES (N'3c6518d8-27ca-48b1-8239-a5d541064b0c', N'User')
GO
INSERT [dbo].[Group] ([Id], [Name]) VALUES (N'fd693b9f-8afb-47f8-9095-a5f43f00e2c7', N'Manager')
GO
INSERT [dbo].[GroupRole] ([Id], [GroupId], [RoleId], [Inherited]) VALUES (N'41f68c2f-e5e2-4c08-a819-7a0e4963c113', N'2eb7e23e-5e2a-4fe8-999f-054b04299632', N'd14b18ba-3653-4942-abe1-eea2e1352296', 0)
GO
INSERT [dbo].[GroupRole] ([Id], [GroupId], [RoleId], [Inherited]) VALUES (N'51190477-40c4-4d9f-bd37-a8c2176429dd', N'fd693b9f-8afb-47f8-9095-a5f43f00e2c7', N'bbf5eed5-9df2-47c1-81c7-11dbe60309ad', 0)
GO
INSERT [dbo].[GroupRole] ([Id], [GroupId], [RoleId], [Inherited]) VALUES (N'a51f539f-b6d7-43d6-9ebb-dd06e4ee52ea', N'3c6518d8-27ca-48b1-8239-a5d541064b0c', N'7b19be67-cfba-4fcc-a806-ff9090fe5091', 0)
GO
INSERT [dbo].[Permission] ([Id], [RoleId], [Module], [Add], [Edit], [View], [Delete]) VALUES (N'70f97fa0-473b-48a5-9009-27351f9de37a', N'7b19be67-cfba-4fcc-a806-ff9090fe5091', N'User', 0, 1, 1, 0)
GO
INSERT [dbo].[Permission] ([Id], [RoleId], [Module], [Add], [Edit], [View], [Delete]) VALUES (N'5ef8d515-663c-436c-bddf-5714be0b6a59', N'd14b18ba-3653-4942-abe1-eea2e1352296', N'User', 1, 1, 1, 1)
GO
INSERT [dbo].[Permission] ([Id], [RoleId], [Module], [Add], [Edit], [View], [Delete]) VALUES (N'fbacf65b-fa04-4a39-8823-ca11312d267f', N'bbf5eed5-9df2-47c1-81c7-11dbe60309ad', N'User', 1, 1, 1, 0)
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'bbf5eed5-9df2-47c1-81c7-11dbe60309ad', N'Manager')
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'd14b18ba-3653-4942-abe1-eea2e1352296', N'SuperAdmin')
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'7b19be67-cfba-4fcc-a806-ff9090fe5091', N'User')
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [GroupId], [CreatedOn]) VALUES (N'f5aa0351-4335-42cc-8146-6fa45d2ecb11', N'Aagam', N'password', N'7894561230', N'aagam@gmail.com', N'3c6518d8-27ca-48b1-8239-a5d541064b0c', CAST(N'2023-03-21T10:52:12.6830000' AS DateTime2))
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [GroupId], [CreatedOn]) VALUES (N'f70f6582-5aeb-4baf-9264-a64437f15af6', N'Manan', N'password', N'11234567890', N'manan@gmail.com', N'fd693b9f-8afb-47f8-9095-a5f43f00e2c7', CAST(N'2023-03-21T10:51:25.2200000' AS DateTime2))
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [GroupId], [CreatedOn]) VALUES (N'c60bfeb9-1e93-43d5-8b8f-f876e6098c98', N'Tushar', N'Password', N'9586541240', N'Tushar@gmail.com', N'2eb7e23e-5e2a-4fe8-999f-054b04299632', CAST(N'2023-03-21T10:51:19.0070000' AS DateTime2))
GO
/****** Object:  Index [IX_GroupRole_GroupId]    Script Date: 2023-03-21 03:44:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_GroupRole_GroupId] ON [dbo].[GroupRole]
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Permission_RoleId]    Script Date: 2023-03-21 03:44:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Permission_RoleId] ON [dbo].[Permission]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GroupRole]  WITH CHECK ADD  CONSTRAINT [FK_GroupRole_Group_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupRole] CHECK CONSTRAINT [FK_GroupRole_Group_GroupId]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK_Permission_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK_Permission_Role_RoleId]
GO
USE [master]
GO
ALTER DATABASE [JWTDemo] SET  READ_WRITE 
GO

USE [EpicMarket]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 28-12-2024 00:47:54 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccessControlLists]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessControlLists](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[AccessTypeID] [int] NOT NULL,
	[SecurableID] [int] NOT NULL,
 CONSTRAINT [PK_AccessControlLists] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccessTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_AccessTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[State] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Pincode] [int] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[MetaData] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationConfigurations]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationConfigurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ApplicationConfigurations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationSecurables]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationSecurables](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ApplicationSecurables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationsTable]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationsTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Sequence] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ApplicationsTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[UniqueGuid] [nvarchar](max) NULL,
	[OTP] [int] NOT NULL,
	[LastActive] [datetime2](7) NOT NULL,
	[AddressId] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttachmentLinks]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttachmentLinks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentID] [int] NOT NULL,
	[EntityID] [int] NOT NULL,
	[RecordID] [int] NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[AttachmentTypeID] [int] NOT NULL,
 CONSTRAINT [PK_AttachmentLinks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attachments]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[DocumentType] [nvarchar](100) NOT NULL,
	[DocumentFileType] [nvarchar](100) NULL,
	[DocumentFolderPath] [nvarchar](500) NULL,
	[DocumentFile] [nvarchar](max) NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttachmentTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttachmentTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_AttachmentTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogCategory]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_BlogCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blogs]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BlogCategoryID] [int] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[InnerHtml] [nvarchar](max) NULL,
	[Authour] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessCategories]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_BusinessCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessEmployeeMaps]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessEmployeeMaps](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BussinessID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BusinessEmployeeMaps] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Businesses]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Businesses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[StatusId] [int] NULL,
	[BusinessCategoryID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Banner] [nvarchar](max) NULL,
	[Logo] [nvarchar](max) NULL,
	[ContactNumber] [bigint] NOT NULL,
	[ContactEmail] [nvarchar](max) NULL,
	[AddressID] [int] NOT NULL,
	[Rating] [int] NULL,
	[ReviewCount] [int] NULL,
	[IsOpen] [bit] NOT NULL,
	[Weight] [float] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Businesses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Catalogs]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Catalogs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BusinessID] [int] NOT NULL,
	[Barcode] [bigint] NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Category] [nvarchar](max) NULL,
	[Rate] [float] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RequiresRefrigeration] [bit] NOT NULL,
	[IsRecommended] [bit] NOT NULL,
	[MaximumOrderPurchase] [int] NULL,
	[Rating] [float] NULL,
	[ReviewCount] [int] NULL,
	[OrderCount] [int] NULL,
	[StatusId] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[CostPrice] [float] NOT NULL,
	[PackedDepth] [float] NOT NULL,
	[PackedHeight] [float] NOT NULL,
	[PackedWidhth] [float] NOT NULL,
	[Weight] [float] NOT NULL,
 CONSTRAINT [PK_Catalogs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentText] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[RecordID] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[EntityID] [int] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommunicationQueue]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommunicationQueue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactMethodID] [int] NULL,
	[MessageData] [nvarchar](max) NULL,
	[Subject] [nvarchar](255) NULL,
	[MessageText] [nvarchar](max) NULL,
	[Attempts] [int] NULL,
	[ScheduledDate] [datetime2](7) NULL,
	[NotificationRecipient] [nvarchar](max) NULL,
	[SysStartTime] [datetime2](7) NULL,
	[SysEndTime] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CommunicationQueue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactMethod]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactMethod](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ContactMethod] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatabaseVersions]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatabaseVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionClass] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_DatabaseVersions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EpicMarketLogs]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EpicMarketLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_EpicMarketLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventCategoryID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[PriorityID] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventLog]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[EntityID] [int] NOT NULL,
	[RecordID] [int] NOT NULL,
	[Source] [nvarchar](255) NULL,
	[Description] [nvarchar](2000) NULL,
	[Data] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAQCategories]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryTitle] [nvarchar](max) NULL,
	[TypeOfFAQ] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_FAQCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAQs]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_FAQs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finances]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OutletID] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Finances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HelpItems]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HelpItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PageID] [int] NULL,
	[IsShownOnPage] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_HelpItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantBankAccounts]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantBankAccounts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MerchantFinanceId] [int] NOT NULL,
	[AccountNumber] [nvarchar](max) NULL,
	[IfscCode] [nvarchar](max) NULL,
	[BankName] [nvarchar](max) NULL,
	[BranchName] [nvarchar](max) NULL,
	[AccountHolderName] [nvarchar](max) NULL,
	[IsPrimaryAccount] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_MerchantBankAccounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantUpiAccounts]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantUpiAccounts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MerchantFinanceId] [int] NOT NULL,
	[UpiIdentifier] [nvarchar](max) NULL,
	[MerchantName] [nvarchar](max) NULL,
	[QrCodeUrl] [nvarchar](max) NULL,
	[IsPrimaryAccount] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_MerchantUpiAccounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[ContactMethodId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OnboardingSteps]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OnboardingSteps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StepName] [nvarchar](max) NOT NULL,
	[StepDescription] [nvarchar](max) NULL,
	[StepOrder] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[PageId] [int] NOT NULL,
 CONSTRAINT [PK_OnboardingSteps] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[CatalogID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Rate] [float] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[OutletID] [int] NOT NULL,
	[OrderTypeId] [int] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[TotalItems] [int] NOT NULL,
	[OrderAt] [datetime2](7) NOT NULL,
	[StatusId] [int] NOT NULL,
	[PaymentMode] [nvarchar](max) NULL,
	[AddressID] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatusOptions]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatusOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatus] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrderStatusOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderTypesOptions]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderTypesOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Ordertype] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrderTypesOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutletPeople]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutletPeople](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[OutletId] [int] NOT NULL,
 CONSTRAINT [PK_OutletPeople] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutletProducts]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutletProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OutletID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[MaximumStockLevel] [int] NOT NULL,
	[MinimumStockLevel] [int] NOT NULL,
	[QuantityAvailable] [int] NOT NULL,
	[ReorderPoint] [int] NOT NULL,
	[BackOrders] [bit] NOT NULL,
 CONSTRAINT [PK_OutletProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Outlets]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Outlets](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BussinessID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[ContactNumber] [bigint] NOT NULL,
	[ContactEmail] [nvarchar](max) NULL,
	[Rating] [float] NULL,
	[ReviewCount] [int] NULL,
	[IsOpen] [bit] NOT NULL,
	[Weight] [float] NULL,
	[StatusId] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Outlets] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ApplicationId] [int] NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[Url] [nvarchar](max) NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_PersonTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductInternals]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductInternals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BarCode] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CostPrice] [float] NOT NULL,
	[PackedDepth] [float] NOT NULL,
	[PackedHeight] [float] NOT NULL,
	[PackedWidhth] [float] NOT NULL,
	[Rate] [float] NOT NULL,
	[Weight] [float] NOT NULL,
 CONSTRAINT [PK_ProductInternals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PromotionalLeads]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromotionalLeads](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Gmail] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[Time] [time](7) NOT NULL,
	[WhichApplication] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PromotionalLeads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proofs]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proofs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityType] [nvarchar](max) NULL,
	[EntityId] [int] NOT NULL,
	[ProofNumber] [nvarchar](max) NULL,
	[ProofTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Proofs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProofTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProofTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ProofTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ratings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[ProductId] [int] NULL,
	[OutletId] [int] NULL,
	[Stars] [int] NOT NULL,
	[Review] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsVerified] [bit] NOT NULL,
 CONSTRAINT [PK_Ratings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusOptionSets]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusOptionSets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](255) NULL,
	[StatusDescription] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_StatusOptionSets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OutletId] [int] NOT NULL,
	[SubscribedDate] [datetime2](7) NOT NULL,
	[UnsubscribedDate] [datetime2](7) NULL,
	[StatusID] [int] NOT NULL,
 CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportQuerys]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportQuerys](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Query] [nvarchar](max) NULL,
	[TaskTypeID] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[TypeofPersonid] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SupportQuerys] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportTickets]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTickets](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Phonenumber] [nvarchar](max) NULL,
	[Fullname] [nvarchar](max) NULL,
	[TypeofPersonid] [int] NOT NULL,
	[Taskid] [int] NULL,
	[AppUserId] [int] NULL,
	[CreateBy] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SupportTickets] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SusbcriptionStatuses]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SusbcriptionStatuses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_SusbcriptionStatuses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[TaskTypeID] [int] NULL,
	[ParentID] [int] NULL,
	[TaskStatusID] [int] NULL,
	[TaskPriorityID] [int] NULL,
	[TaskEntityID] [int] NULL,
	[PrimaryAssignedToPersonID] [int] NULL,
	[DateAssigned] [datetime2](7) NULL,
	[DateDue] [datetime2](7) NULL,
	[DateStarted] [datetime2](7) NULL,
	[DateCompleted] [datetime2](7) NULL,
	[SubmittedByPersonID] [int] NULL,
	[TaskData] [nvarchar](max) NULL,
	[ReceivedDate] [datetime2](7) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskStatusTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskStatusTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](255) NULL,
	[StatusDescription] [nvarchar](max) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TaskStatusTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskTypes]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
	[TaskCategoryID] [int] NULL,
	[DefaultDueDateHours] [int] NULL,
	[ShortDescription] [nvarchar](20) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[CreateBy] [nvarchar](max) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TaskTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAddresses]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAddresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
 CONSTRAINT [PK_UserAddresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserOnboardingProgresses]    Script Date: 28-12-2024 00:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserOnboardingProgresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[StepID] [int] NOT NULL,
	[CompletedAt] [datetime2](7) NULL,
	[IsCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserOnboardingProgresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Addresses] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Addresses] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ApplicationConfigurations] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ApplicationConfigurations] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[ApplicationConfigurations] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ApplicationSecurables] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ApplicationSecurables] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[ApplicationSecurables] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ApplicationsTable] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ApplicationsTable] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[ApplicationsTable] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[AttachmentLinks] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[AttachmentLinks] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[AttachmentLinks] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AttachmentLinks] ADD  DEFAULT ((0)) FOR [AttachmentTypeID]
GO
ALTER TABLE [dbo].[Attachments] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Attachments] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Attachments] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BlogCategory] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[BlogCategory] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[BlogCategory] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Blogs] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Blogs] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Blogs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BusinessCategories] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[BusinessCategories] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[BusinessCategories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Businesses] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Businesses] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Businesses] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [CostPrice]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedDepth]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedHeight]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedWidhth]
GO
ALTER TABLE [dbo].[Catalogs] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Weight]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT ((0)) FOR [EntityID]
GO
ALTER TABLE [dbo].[CommunicationQueue] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[CommunicationQueue] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[CommunicationQueue] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ContactMethod] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ContactMethod] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[ContactMethod] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Entity] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Entity] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Entity] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Event] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Event] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Event] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[EventLog] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[EventLog] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[EventLog] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FAQCategories] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[FAQCategories] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[FAQCategories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FAQs] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[FAQs] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[FAQs] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[HelpItems] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[HelpItems] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[HelpItems] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [ContactMethodId]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ((0)) FOR [PageId]
GO
ALTER TABLE [dbo].[OnboardingSteps] ADD  DEFAULT ((0)) FOR [PageId]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[OutletProducts] ADD  DEFAULT ((0)) FOR [MaximumStockLevel]
GO
ALTER TABLE [dbo].[OutletProducts] ADD  DEFAULT ((0)) FOR [MinimumStockLevel]
GO
ALTER TABLE [dbo].[OutletProducts] ADD  DEFAULT ((0)) FOR [QuantityAvailable]
GO
ALTER TABLE [dbo].[OutletProducts] ADD  DEFAULT ((0)) FOR [ReorderPoint]
GO
ALTER TABLE [dbo].[OutletProducts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [BackOrders]
GO
ALTER TABLE [dbo].[Outlets] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsOpen]
GO
ALTER TABLE [dbo].[Outlets] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Outlets] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Outlets] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Pages] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Pages] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Pages] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [CostPrice]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedDepth]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedHeight]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [PackedWidhth]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Rate]
GO
ALTER TABLE [dbo].[ProductInternals] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [Weight]
GO
ALTER TABLE [dbo].[StatusOptionSets] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[StatusOptionSets] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[StatusOptionSets] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SupportQuerys] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[SupportQuerys] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[SupportQuerys] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SupportTickets] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[SupportTickets] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[SupportTickets] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TaskStatusTypes] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TaskStatusTypes] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[TaskStatusTypes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TaskTypes] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TaskTypes] ADD  DEFAULT ('System') FOR [CreateBy]
GO
ALTER TABLE [dbo].[TaskTypes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AccessControlLists]  WITH CHECK ADD  CONSTRAINT [FK_AccessControlLists_AccessTypes_AccessTypeID] FOREIGN KEY([AccessTypeID])
REFERENCES [dbo].[AccessTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccessControlLists] CHECK CONSTRAINT [FK_AccessControlLists_AccessTypes_AccessTypeID]
GO
ALTER TABLE [dbo].[AccessControlLists]  WITH CHECK ADD  CONSTRAINT [FK_AccessControlLists_ApplicationSecurables_SecurableID] FOREIGN KEY([SecurableID])
REFERENCES [dbo].[ApplicationSecurables] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccessControlLists] CHECK CONSTRAINT [FK_AccessControlLists_ApplicationSecurables_SecurableID]
GO
ALTER TABLE [dbo].[AccessControlLists]  WITH CHECK ADD  CONSTRAINT [FK_AccessControlLists_AspNetRoles_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccessControlLists] CHECK CONSTRAINT [FK_AccessControlLists_AspNetRoles_RoleID]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_Addresses_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_Addresses_AddressId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AttachmentLinks]  WITH CHECK ADD  CONSTRAINT [FK_AttachmentLinks_Attachments_AttachmentID] FOREIGN KEY([AttachmentID])
REFERENCES [dbo].[Attachments] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AttachmentLinks] CHECK CONSTRAINT [FK_AttachmentLinks_Attachments_AttachmentID]
GO
ALTER TABLE [dbo].[AttachmentLinks]  WITH CHECK ADD  CONSTRAINT [FK_AttachmentLinks_AttachmentTypes_AttachmentTypeID] FOREIGN KEY([AttachmentTypeID])
REFERENCES [dbo].[AttachmentTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AttachmentLinks] CHECK CONSTRAINT [FK_AttachmentLinks_AttachmentTypes_AttachmentTypeID]
GO
ALTER TABLE [dbo].[AttachmentLinks]  WITH CHECK ADD  CONSTRAINT [FK_AttachmentLinks_Entity_EntityID] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AttachmentLinks] CHECK CONSTRAINT [FK_AttachmentLinks_Entity_EntityID]
GO
ALTER TABLE [dbo].[Blogs]  WITH CHECK ADD  CONSTRAINT [FK_Blogs_BlogCategory_BlogCategoryID] FOREIGN KEY([BlogCategoryID])
REFERENCES [dbo].[BlogCategory] ([Id])
GO
ALTER TABLE [dbo].[Blogs] CHECK CONSTRAINT [FK_Blogs_BlogCategory_BlogCategoryID]
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployeeMaps_AspNetUsers_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps] CHECK CONSTRAINT [FK_BusinessEmployeeMaps_AspNetUsers_EmployeeID]
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployeeMaps_Businesses_BussinessID] FOREIGN KEY([BussinessID])
REFERENCES [dbo].[Businesses] ([ID])
GO
ALTER TABLE [dbo].[BusinessEmployeeMaps] CHECK CONSTRAINT [FK_BusinessEmployeeMaps_Businesses_BussinessID]
GO
ALTER TABLE [dbo].[Businesses]  WITH CHECK ADD  CONSTRAINT [FK_Businesses_Addresses_AddressID] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Businesses] CHECK CONSTRAINT [FK_Businesses_Addresses_AddressID]
GO
ALTER TABLE [dbo].[Businesses]  WITH CHECK ADD  CONSTRAINT [FK_Businesses_AspNetUsers_PersonID] FOREIGN KEY([PersonID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Businesses] CHECK CONSTRAINT [FK_Businesses_AspNetUsers_PersonID]
GO
ALTER TABLE [dbo].[Businesses]  WITH CHECK ADD  CONSTRAINT [FK_Businesses_BusinessCategories_BusinessCategoryID] FOREIGN KEY([BusinessCategoryID])
REFERENCES [dbo].[BusinessCategories] ([ID])
GO
ALTER TABLE [dbo].[Businesses] CHECK CONSTRAINT [FK_Businesses_BusinessCategories_BusinessCategoryID]
GO
ALTER TABLE [dbo].[Businesses]  WITH CHECK ADD  CONSTRAINT [FK_Businesses_StatusOptionSets_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StatusOptionSets] ([Id])
GO
ALTER TABLE [dbo].[Businesses] CHECK CONSTRAINT [FK_Businesses_StatusOptionSets_StatusId]
GO
ALTER TABLE [dbo].[Catalogs]  WITH CHECK ADD  CONSTRAINT [FK_Catalogs_Businesses_BusinessID] FOREIGN KEY([BusinessID])
REFERENCES [dbo].[Businesses] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Catalogs] CHECK CONSTRAINT [FK_Catalogs_Businesses_BusinessID]
GO
ALTER TABLE [dbo].[Catalogs]  WITH CHECK ADD  CONSTRAINT [FK_Catalogs_StatusOptionSets_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StatusOptionSets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Catalogs] CHECK CONSTRAINT [FK_Catalogs_StatusOptionSets_StatusId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Entity_EntityID] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Entity_EntityID]
GO
ALTER TABLE [dbo].[CommunicationQueue]  WITH CHECK ADD  CONSTRAINT [FK_CommunicationQueue_ContactMethod_ContactMethodID] FOREIGN KEY([ContactMethodID])
REFERENCES [dbo].[ContactMethod] ([ID])
GO
ALTER TABLE [dbo].[CommunicationQueue] CHECK CONSTRAINT [FK_CommunicationQueue_ContactMethod_ContactMethodID]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_ApplicationsTable_EventCategoryID] FOREIGN KEY([EventCategoryID])
REFERENCES [dbo].[ApplicationsTable] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_ApplicationsTable_EventCategoryID]
GO
ALTER TABLE [dbo].[EventLog]  WITH CHECK ADD  CONSTRAINT [FK_EventLog_Entity_EntityID] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventLog] CHECK CONSTRAINT [FK_EventLog_Entity_EntityID]
GO
ALTER TABLE [dbo].[EventLog]  WITH CHECK ADD  CONSTRAINT [FK_EventLog_Event_EventID] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventLog] CHECK CONSTRAINT [FK_EventLog_Event_EventID]
GO
ALTER TABLE [dbo].[FAQs]  WITH CHECK ADD  CONSTRAINT [FK_FAQs_FAQCategories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[FAQCategories] ([Id])
GO
ALTER TABLE [dbo].[FAQs] CHECK CONSTRAINT [FK_FAQs_FAQCategories_CategoryId]
GO
ALTER TABLE [dbo].[Finances]  WITH CHECK ADD  CONSTRAINT [FK_Finances_Outlets_OutletID] FOREIGN KEY([OutletID])
REFERENCES [dbo].[Outlets] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Finances] CHECK CONSTRAINT [FK_Finances_Outlets_OutletID]
GO
ALTER TABLE [dbo].[HelpItems]  WITH CHECK ADD  CONSTRAINT [FK_HelpItems_Pages_PageID] FOREIGN KEY([PageID])
REFERENCES [dbo].[Pages] ([ID])
GO
ALTER TABLE [dbo].[HelpItems] CHECK CONSTRAINT [FK_HelpItems_Pages_PageID]
GO
ALTER TABLE [dbo].[MerchantBankAccounts]  WITH CHECK ADD  CONSTRAINT [FK_MerchantBankAccounts_Finances_MerchantFinanceId] FOREIGN KEY([MerchantFinanceId])
REFERENCES [dbo].[Finances] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MerchantBankAccounts] CHECK CONSTRAINT [FK_MerchantBankAccounts_Finances_MerchantFinanceId]
GO
ALTER TABLE [dbo].[MerchantUpiAccounts]  WITH CHECK ADD  CONSTRAINT [FK_MerchantUpiAccounts_Finances_MerchantFinanceId] FOREIGN KEY([MerchantFinanceId])
REFERENCES [dbo].[Finances] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MerchantUpiAccounts] CHECK CONSTRAINT [FK_MerchantUpiAccounts_Finances_MerchantFinanceId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_ContactMethod_ContactMethodId] FOREIGN KEY([ContactMethodId])
REFERENCES [dbo].[ContactMethod] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_ContactMethod_ContactMethodId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Pages_PageId] FOREIGN KEY([PageId])
REFERENCES [dbo].[Pages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Pages_PageId]
GO
ALTER TABLE [dbo].[OnboardingSteps]  WITH CHECK ADD  CONSTRAINT [FK_OnboardingSteps_Pages_PageId] FOREIGN KEY([PageId])
REFERENCES [dbo].[Pages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OnboardingSteps] CHECK CONSTRAINT [FK_OnboardingSteps_Pages_PageId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Catalogs_CatalogID] FOREIGN KEY([CatalogID])
REFERENCES [dbo].[Catalogs] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Catalogs_CatalogID]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders_OrderID] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders_OrderID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Addresses_AddressID] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Addresses_AddressID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_AspNetUsers_PersonID] FOREIGN KEY([PersonID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_AspNetUsers_PersonID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatusOptions_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrderStatusOptions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatusOptions_StatusId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderTypesOptions_OrderTypeId] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderTypesOptions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderTypesOptions_OrderTypeId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Outlets_OutletID] FOREIGN KEY([OutletID])
REFERENCES [dbo].[Outlets] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Outlets_OutletID]
GO
ALTER TABLE [dbo].[OutletPeople]  WITH CHECK ADD  CONSTRAINT [FK_OutletPeople_AspNetUsers_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[OutletPeople] CHECK CONSTRAINT [FK_OutletPeople_AspNetUsers_PersonId]
GO
ALTER TABLE [dbo].[OutletPeople]  WITH CHECK ADD  CONSTRAINT [FK_OutletPeople_Outlets_OutletId] FOREIGN KEY([OutletId])
REFERENCES [dbo].[Outlets] ([ID])
GO
ALTER TABLE [dbo].[OutletPeople] CHECK CONSTRAINT [FK_OutletPeople_Outlets_OutletId]
GO
ALTER TABLE [dbo].[OutletProducts]  WITH CHECK ADD  CONSTRAINT [FK_OutletProducts_Catalogs_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Catalogs] ([ID])
GO
ALTER TABLE [dbo].[OutletProducts] CHECK CONSTRAINT [FK_OutletProducts_Catalogs_ProductID]
GO
ALTER TABLE [dbo].[OutletProducts]  WITH CHECK ADD  CONSTRAINT [FK_OutletProducts_Outlets_OutletID] FOREIGN KEY([OutletID])
REFERENCES [dbo].[Outlets] ([ID])
GO
ALTER TABLE [dbo].[OutletProducts] CHECK CONSTRAINT [FK_OutletProducts_Outlets_OutletID]
GO
ALTER TABLE [dbo].[Outlets]  WITH CHECK ADD  CONSTRAINT [FK_Outlets_Addresses_AddressID] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Addresses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Outlets] CHECK CONSTRAINT [FK_Outlets_Addresses_AddressID]
GO
ALTER TABLE [dbo].[Outlets]  WITH CHECK ADD  CONSTRAINT [FK_Outlets_Businesses_BussinessID] FOREIGN KEY([BussinessID])
REFERENCES [dbo].[Businesses] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Outlets] CHECK CONSTRAINT [FK_Outlets_Businesses_BussinessID]
GO
ALTER TABLE [dbo].[Outlets]  WITH CHECK ADD  CONSTRAINT [FK_Outlets_StatusOptionSets_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StatusOptionSets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Outlets] CHECK CONSTRAINT [FK_Outlets_StatusOptionSets_StatusId]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_Pages_ApplicationsTable_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationsTable] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_Pages_ApplicationsTable_ApplicationId]
GO
ALTER TABLE [dbo].[Proofs]  WITH CHECK ADD  CONSTRAINT [FK_Proofs_ProofTypes_ProofTypeId] FOREIGN KEY([ProofTypeId])
REFERENCES [dbo].[ProofTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Proofs] CHECK CONSTRAINT [FK_Proofs_ProofTypes_ProofTypeId]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_AspNetUsers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_AspNetUsers_CustomerId]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_Catalogs_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Catalogs] ([ID])
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_Catalogs_ProductId]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_Outlets_OutletId] FOREIGN KEY([OutletId])
REFERENCES [dbo].[Outlets] ([ID])
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_Outlets_OutletId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_AspNetUsers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_AspNetUsers_CustomerId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Outlets_OutletId] FOREIGN KEY([OutletId])
REFERENCES [dbo].[Outlets] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Outlets_OutletId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_SusbcriptionStatuses_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[SusbcriptionStatuses] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_SusbcriptionStatuses_StatusID]
GO
ALTER TABLE [dbo].[SupportQuerys]  WITH CHECK ADD  CONSTRAINT [FK_SupportQuerys_PersonTypes_TypeofPersonid] FOREIGN KEY([TypeofPersonid])
REFERENCES [dbo].[PersonTypes] ([ID])
GO
ALTER TABLE [dbo].[SupportQuerys] CHECK CONSTRAINT [FK_SupportQuerys_PersonTypes_TypeofPersonid]
GO
ALTER TABLE [dbo].[SupportQuerys]  WITH CHECK ADD  CONSTRAINT [FK_SupportQuerys_TaskTypes_TaskTypeID] FOREIGN KEY([TaskTypeID])
REFERENCES [dbo].[TaskTypes] ([ID])
GO
ALTER TABLE [dbo].[SupportQuerys] CHECK CONSTRAINT [FK_SupportQuerys_TaskTypes_TaskTypeID]
GO
ALTER TABLE [dbo].[SupportTickets]  WITH CHECK ADD  CONSTRAINT [FK_SupportTickets_AspNetUsers_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[SupportTickets] CHECK CONSTRAINT [FK_SupportTickets_AspNetUsers_AppUserId]
GO
ALTER TABLE [dbo].[SupportTickets]  WITH CHECK ADD  CONSTRAINT [FK_SupportTickets_PersonTypes_TypeofPersonid] FOREIGN KEY([TypeofPersonid])
REFERENCES [dbo].[PersonTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SupportTickets] CHECK CONSTRAINT [FK_SupportTickets_PersonTypes_TypeofPersonid]
GO
ALTER TABLE [dbo].[SupportTickets]  WITH CHECK ADD  CONSTRAINT [FK_SupportTickets_Tasks_Taskid] FOREIGN KEY([Taskid])
REFERENCES [dbo].[Tasks] ([ID])
GO
ALTER TABLE [dbo].[SupportTickets] CHECK CONSTRAINT [FK_SupportTickets_Tasks_Taskid]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_AspNetUsers_PrimaryAssignedToPersonID] FOREIGN KEY([PrimaryAssignedToPersonID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_AspNetUsers_PrimaryAssignedToPersonID]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Entity_TaskEntityID] FOREIGN KEY([TaskEntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_Entity_TaskEntityID]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_TaskStatusTypes_TaskStatusID] FOREIGN KEY([TaskStatusID])
REFERENCES [dbo].[TaskStatusTypes] ([Id])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_TaskStatusTypes_TaskStatusID]
GO
ALTER TABLE [dbo].[Tasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_TaskTypes_TaskTypeID] FOREIGN KEY([TaskTypeID])
REFERENCES [dbo].[TaskTypes] ([ID])
GO
ALTER TABLE [dbo].[Tasks] CHECK CONSTRAINT [FK_Tasks_TaskTypes_TaskTypeID]
GO
ALTER TABLE [dbo].[TaskTypes]  WITH CHECK ADD  CONSTRAINT [FK_TaskTypes_ApplicationsTable_TaskCategoryID] FOREIGN KEY([TaskCategoryID])
REFERENCES [dbo].[ApplicationsTable] ([ID])
GO
ALTER TABLE [dbo].[TaskTypes] CHECK CONSTRAINT [FK_TaskTypes_ApplicationsTable_TaskCategoryID]
GO
ALTER TABLE [dbo].[UserAddresses]  WITH CHECK ADD  CONSTRAINT [FK_UserAddresses_Addresses_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[UserAddresses] CHECK CONSTRAINT [FK_UserAddresses_Addresses_AddressId]
GO
ALTER TABLE [dbo].[UserAddresses]  WITH CHECK ADD  CONSTRAINT [FK_UserAddresses_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserAddresses] CHECK CONSTRAINT [FK_UserAddresses_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[UserOnboardingProgresses]  WITH CHECK ADD  CONSTRAINT [FK_UserOnboardingProgresses_AspNetUsers_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserOnboardingProgresses] CHECK CONSTRAINT [FK_UserOnboardingProgresses_AspNetUsers_UserID]
GO
ALTER TABLE [dbo].[UserOnboardingProgresses]  WITH CHECK ADD  CONSTRAINT [FK_UserOnboardingProgresses_OnboardingSteps_StepID] FOREIGN KEY([StepID])
REFERENCES [dbo].[OnboardingSteps] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserOnboardingProgresses] CHECK CONSTRAINT [FK_UserOnboardingProgresses_OnboardingSteps_StepID]
GO

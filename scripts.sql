USE [TicTacToe]
GO
/****** Object:  Table [dbo].[game_info]    Script Date: 9/14/2018 8:11:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[game_info](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[player_one_api_key] [varchar](50) NOT NULL,
	[player_two_api_key] [varchar](50) NOT NULL,
	[moves] [int] NOT NULL,
	[status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_game_info] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[log_details]    Script Date: 9/14/2018 8:11:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[log_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[request] [varchar](50) NOT NULL,
	[response] [varchar](50) NOT NULL,
	[exception] [varchar](50) NOT NULL,
	[comment] [varchar](50) NOT NULL,
 CONSTRAINT [PK_log_details] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[user_details]    Script Date: 9/14/2018 8:11:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user_details](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](50) NOT NULL,
	[last_name] [varchar](50) NOT NULL,
	[user_name] [varchar](50) NOT NULL,
	[api_key] [varchar](50) NULL,
 CONSTRAINT [PK_user_details] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

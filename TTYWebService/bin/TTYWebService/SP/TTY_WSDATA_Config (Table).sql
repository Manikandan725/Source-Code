/****** Object:  Table [dbo].[TTY_WSDATA_Config]    Script Date: 10-16-2023 18:24:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TTY_WSDATA_Config](
	[TTYId] [int] IDENTITY(1,1) NOT NULL,
	[CustId] [int] NOT NULL,
	[ProjId] [int] NOT NULL,
	[FormId] [int] NOT NULL,
	[SubprocessId] [int] NOT NULL,
	[ProcessId] [int] NOT NULL,
	[ServiceUrl] [varchar](100) NULL,
	[UserName] [varchar](100) NULL,
	[PassWord] [varchar](100) NULL,
	[Active] [varchar](1) NULL,
	[TTYStatus] [varchar](100) NULL,
 CONSTRAINT [PK__TTY_WSDATA_Config__1DE57479] PRIMARY KEY CLUSTERED 
(
	[TTYId] ASC,
	[CustId] ASC,
	[ProjId] ASC,
	[FormId] ASC,
	[ProcessId] ASC,
	[SubprocessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



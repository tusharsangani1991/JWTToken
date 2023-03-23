USE [JWTRoleDemo]
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'bbf5eed5-9df2-47c1-81c7-11dbe60309ad', N'Manager')
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'006fefe2-63e3-454b-8adb-32b587047c13', N'Client')
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'd14b18ba-3653-4942-abe1-eea2e1352296', N'SuperAdmin')
GO
INSERT [dbo].[Role] ([Id], [Role]) VALUES (N'7b19be67-cfba-4fcc-a806-ff9090fe5091', N'User')
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [CreatedOn]) VALUES (N'f5aa0351-4335-42cc-8146-6fa45d2ecb11', N'Aagam', N'password', N'7894561230', N'aagam@gmail.com', CAST(N'2023-03-21T10:52:12.6830000' AS DateTime2))
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [CreatedOn]) VALUES (N'f70f6582-5aeb-4baf-9264-a64437f15af6', N'Manan', N'password', N'11234567890', N'manan@gmail.com', CAST(N'2023-03-21T10:51:25.2200000' AS DateTime2))
GO
INSERT [dbo].[Users] ([Id], [UserName], [Password], [PhoneNumber], [Email], [CreatedOn]) VALUES (N'c60bfeb9-1e93-43d5-8b8f-f876e6098c98', N'Tushar', N'Password', N'9586541240', N'Tushar@gmail.com', CAST(N'2023-03-21T10:51:19.0070000' AS DateTime2))
GO
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (N'8fa0a390-1457-4509-93b5-323fc0de17de', N'f70f6582-5aeb-4baf-9264-a64437f15af6', N'7b19be67-cfba-4fcc-a806-ff9090fe5091')
GO
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (N'd4c10ae4-006b-4126-9dc6-4b1119af2cd2', N'f5aa0351-4335-42cc-8146-6fa45d2ecb11', N'bbf5eed5-9df2-47c1-81c7-11dbe60309ad')
GO
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (N'30019298-c20b-4bcb-b9a4-9bade2782896', N'c60bfeb9-1e93-43d5-8b8f-f876e6098c98', N'd14b18ba-3653-4942-abe1-eea2e1352296')
GO
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (N'ce3aa90d-b645-4c7a-82e4-d2008c47ba28', N'f70f6582-5aeb-4baf-9264-a64437f15af6', N'006fefe2-63e3-454b-8adb-32b587047c13')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230323161126_Initial', N'6.0.6')
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductDescription], [ProductCost], [ProductStock]) VALUES (1, N'Milk', N'Milk Bottol', 30, 50)
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductDescription], [ProductCost], [ProductStock]) VALUES (2, N'Watch', N'Writch Watch', 895, 52)
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductDescription], [ProductCost], [ProductStock]) VALUES (3, N'KeyBoard', N'Usb KetBorad', 560, 62)
GO
INSERT [dbo].[Products] ([Id], [ProductName], [ProductDescription], [ProductCost], [ProductStock]) VALUES (4, N'Mouse', N'Mouse Wired', 201, 52)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO

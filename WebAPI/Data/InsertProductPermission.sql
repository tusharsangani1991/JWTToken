USE [JWTDemo]
GO

INSERT INTO [dbo].[Permission]
           ([Id]
           ,[RoleId]
           ,[Module]
           ,[Add]
           ,[Edit]
           ,[View]
           ,[Delete])
     VALUES
           (NEWID()
           ,'7B19BE67-CFBA-4FCC-A806-FF9090FE5091'
           ,'Product'
           ,0
           ,1
           ,1
           ,0),
		   (NEWID()
           ,'D14B18BA-3653-4942-ABE1-EEA2E1352296'
           ,'Product'
           ,1
           ,1
           ,1
           ,1),
		   (NEWID()
           ,'BBF5EED5-9DF2-47C1-81C7-11DBE60309AD'
           ,'Product'
           ,1
           ,1
           ,1
           ,0)
GO



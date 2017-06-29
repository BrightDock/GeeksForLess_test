CREATE TABLE [dbo].[Users]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Last_name] NVARCHAR(50) NULL, 
    [Nick_name] NVARCHAR(30) NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [PasswordHash] NVARCHAR(500) NOT NULL, 
    [Avatar] NVARCHAR(200) NULL, 
    [User_type] INT NOT NULL, 
    [SecurityStamp] NVARCHAR(254) NULL, 
    [Descriminator] NCHAR(254) NULL, 
    [isConfirmed] BIT NOT NULL DEFAULT 1
)

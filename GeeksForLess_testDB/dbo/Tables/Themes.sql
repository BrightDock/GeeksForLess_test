CREATE TABLE [dbo].[Themes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(150) NOT NULL, 
    [Author] NVARCHAR(128) NOT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [Main_theme] BIGINT NULL, 
    [Publication_date] DATETIME2 NULL, 
    CONSTRAINT [FK_Themes_Users] FOREIGN KEY (Author) REFERENCES AspNetUsers(Id) ON DELETE CASCADE, 
    CONSTRAINT [FK_Themes_Themes] FOREIGN KEY (Main_theme) REFERENCES Themes(Id)
)

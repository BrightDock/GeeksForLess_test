CREATE TABLE [dbo].[Themes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(150) NOT NULL, 
    [Author] BIGINT NOT NULL, 
    [Main_theme] BIGINT NULL, 
    CONSTRAINT [FK_Themes_Users] FOREIGN KEY (Author) REFERENCES Users(Id), 
    CONSTRAINT [FK_Themes_Themes] FOREIGN KEY (Main_theme) REFERENCES Themes(Id)
)

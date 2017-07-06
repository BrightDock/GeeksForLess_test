CREATE TABLE [dbo].[Themes_messages]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Theme] BIGINT NOT NULL, 
    [Author] NVARCHAR(128) NOT NULL, 
    [Reply_to] BIGINT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    [Publication_date] DATETIME2 NULL, 
    CONSTRAINT [FK_Themes_messages_Themes] FOREIGN KEY (Theme) REFERENCES Themes(Id) ON DELETE CASCADE, 
    CONSTRAINT [FK_Themes_messages_Users] FOREIGN KEY (Author) REFERENCES AspNetUsers(Id) ON DELETE CASCADE, 
    CONSTRAINT [FK_Themes_messages_Themes_messages] FOREIGN KEY (Reply_to) REFERENCES Themes_messages(Id) 
)

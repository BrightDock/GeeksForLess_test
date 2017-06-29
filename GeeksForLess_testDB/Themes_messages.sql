CREATE TABLE [dbo].[Themes_messages]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Theme] BIGINT NOT NULL, 
    [Author] BIGINT NOT NULL, 
    CONSTRAINT [FK_Themes_messages_Themes] FOREIGN KEY (Theme) REFERENCES Themes(Id), 
    CONSTRAINT [FK_Themes_messages_Users] FOREIGN KEY (Author) REFERENCES Users(Id) 
)

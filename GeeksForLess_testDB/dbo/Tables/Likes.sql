CREATE TABLE [dbo].[Likes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Target] BIGINT NOT NULL, 
    [Target_type] INT NOT NULL, 
    [Like_author] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [FK_Likes_Likes_types] FOREIGN KEY (Target_type) REFERENCES Likes_targets(Id), 
    CONSTRAINT [FK_Likes_Users] FOREIGN KEY (Like_author) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
)

CREATE TABLE [dbo].[Options]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Option_name] NVARCHAR(100) NOT NULL, 
    [Option_value] NVARCHAR(100) NOT NULL
)

CREATE TABLE [dbo].[User]
(
	[User_id]	UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Username]  NVARCHAR(255)    NOT NULL, 
    [Password]	NVARCHAR(255)	 NOT NULL, 
    [Admin]		BIT				 NOT NULL DEFAULT 0, 
    [Email]		NCHAR(255)		 NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UNIQUE',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Email'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UNIQUE',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Username'
CREATE TABLE [dbo].[Account]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[UserId] nvarchar(128) not null,
	[Name] nvarchar(50) null,
	[Type] nvarchar(20) null,
	[DateCreated] datetime2 not null default getutcdate(),
	[Balance] MONEY null
)

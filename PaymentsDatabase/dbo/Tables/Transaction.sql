CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[AccountId] INT not null,
	[Status] nvarchar(20) not null,
	[Amount] MONEY not null default 0,
	[TransactionFee] MONEY null default 0,
	[Note] nvarchar(20) null,
	[DateCreated] datetime2 not null default getutcdate(),
	[TransactionDate] datetime2 not null default getutcdate(),
)

IF DB_ID('PodcastDB_Group17') IS NULL
    CREATE DATABASE PodcastDB_Group17;
GO
USE PodcastDB_Group17;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Role] int NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Podcasts] (
    [PodcastID] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatorID] nvarchar(450) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Podcasts] PRIMARY KEY ([PodcastID]),
    CONSTRAINT [FK_Podcasts_AspNetUsers_CreatorID] FOREIGN KEY ([CreatorID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Episodes] (
    [EpisodeID] int NOT NULL IDENTITY,
    [PodcastID] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [Duration] int NOT NULL,
    [PlayCount] int NOT NULL,
    [AudioFileURL] nvarchar(max) NOT NULL,
    [VideoFileURL] nvarchar(max) NOT NULL,
    [Topic] nvarchar(max) NOT NULL,
    [Host] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Episodes] PRIMARY KEY ([EpisodeID]),
    CONSTRAINT [FK_Episodes_Podcasts_PodcastID] FOREIGN KEY ([PodcastID]) REFERENCES [Podcasts] ([PodcastID]) ON DELETE CASCADE
);

CREATE TABLE [Subscriptions] (
    [SubscriptionID] int NOT NULL IDENTITY,
    [UserID] nvarchar(450) NOT NULL,
    [PodcastID] int NOT NULL,
    [SubscribedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([SubscriptionID]),
    CONSTRAINT [FK_Subscriptions_AspNetUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Subscriptions_Podcasts_PodcastID] FOREIGN KEY ([PodcastID]) REFERENCES [Podcasts] ([PodcastID]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
CREATE INDEX [IX_Episodes_PodcastID] ON [Episodes] ([PodcastID]);
CREATE INDEX [IX_Podcasts_CreatorID] ON [Podcasts] ([CreatorID]);
CREATE INDEX [IX_Subscriptions_PodcastID] ON [Subscriptions] ([PodcastID]);
CREATE INDEX [IX_Subscriptions_UserID] ON [Subscriptions] ([UserID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251025150937_InitialCreate', N'8.0.20');
GO

COMMIT;
GO

INSERT INTO AspNetUsers (Id,Role,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount) VALUES
(N'00000000-0000-0000-0000-000000000001',1,N'admin@podcast.local',N'ADMIN@PODCAST.LOCAL',N'admin@podcast.local',N'ADMIN@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0),
(N'00000000-0000-0000-0000-000000000010',0,N'ohm',N'OHM',N'ohm@podcast.local',N'OHM@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0),
(N'00000000-0000-0000-0000-000000000011',0,N'harvend',N'HARVEND',N'harvend@podcast.local',N'HARVEND@PODCAST.LOCAL',1,NULL,NEWID(),NEWID(),0,0,0,0);

INSERT INTO Podcasts (Title,Description,CreatorID,CreatedDate) VALUES
(N'6 Minute English',N'A short English-learning podcast with bite-sized episodes',N'00000000-0000-0000-0000-000000000010','2017-09-07'),
(N'8 Minute English',N'Practical English lessons in 8 minutes',N'00000000-0000-0000-0000-000000000010','2024-01-28'),
(N'To Your Inner Child',N'Guided conversations about inner child work',N'00000000-0000-0000-0000-000000000011','2024-03-20'),
(N'Podcast and Chill',N'Relaxed conversations about language learning and memory',N'00000000-0000-0000-0000-000000000011','2024-01-26'),
(N'Learning English for Work',N'English for the workplace: emails, meetings and more',N'00000000-0000-0000-0000-000000000001','2024-04-28');

INSERT INTO Episodes (PodcastID,Title,ReleaseDate,Duration,PlayCount,AudioFileURL,VideoFileURL,Topic,Host) VALUES
((SELECT PodcastID FROM Podcasts WHERE Title=N'6 Minute English'),N'The benefits of doing nothing','2023-06-15',22,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/1.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/1.mp4',N'Wellbeing',N'BBC Learning English'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'6 Minute English'),N'Did a civilisation exist on Earth before humans','2025-10-23',26,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/2.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/2.mp4',N'Prehistory',N'BBC Learning English'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'8 Minute English'),N'Why Do You Always Feel Tired','2024-12-21',8,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/3.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/3.mp4',N'Health',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'To Your Inner Child'),N'Everything Happens For A Reason','2024-03-20',30,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/4.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/4.mp4',N'Personal Growth',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'Podcast and Chill'),N'Why We Can''t Remember New Words','2024-06-25',28,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/5.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/5.mp4',N'Memory & Language',N'Learn English Podcast'),
((SELECT PodcastID FROM Podcasts WHERE Title=N'Learning English for Work'),N'Work emails — Office English (Episode 1)','2024-04-28',18,0,N'https://podcast-lab3.s3.us-east-2.amazonaws.com/6.mp3',N'https://podcast-lab3.s3.us-east-2.amazonaws.com/6.mp4',N'Business English',N'BBC Learning English');

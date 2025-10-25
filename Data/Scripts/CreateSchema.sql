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

CREATE TABLE [Podcasts] (
    [PodcastID] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(2000) NULL,
    [CreatorID] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Podcasts] PRIMARY KEY ([PodcastID])
);
GO

CREATE TABLE [Episodes] (
    [EpisodeID] int NOT NULL IDENTITY,
    [PodcastID] int NOT NULL,
    [Title] nvarchar(200) NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [Duration] int NOT NULL,
    [PlayCount] int NOT NULL,
    [AudioFileURL] nvarchar(500) NULL,
    [VideoFileURL] nvarchar(500) NULL,
    [Topic] nvarchar(100) NULL,
    [Host] nvarchar(100) NULL,
    CONSTRAINT [PK_Episodes] PRIMARY KEY ([EpisodeID]),
    CONSTRAINT [FK_Episodes_Podcasts_PodcastID] FOREIGN KEY ([PodcastID]) REFERENCES [Podcasts] ([PodcastID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Subscriptions] (
    [SubscriptionID] int NOT NULL IDENTITY,
    [UserID] nvarchar(max) NOT NULL,
    [PodcastID] int NOT NULL,
    [SubscribedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([SubscriptionID]),
    CONSTRAINT [FK_Subscriptions_Podcasts_PodcastID] FOREIGN KEY ([PodcastID]) REFERENCES [Podcasts] ([PodcastID]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Episodes_PodcastID] ON [Episodes] ([PodcastID]);
GO

CREATE INDEX [IX_Subscriptions_PodcastID] ON [Subscriptions] ([PodcastID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251025033635_InitialPodcastSchema', N'8.0.20');
GO

COMMIT;
GO


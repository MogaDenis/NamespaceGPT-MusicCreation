	IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MusicDB')
BEGIN
    CREATE DATABASE [MusicDB]
END

IF NOT EXISTS(SELECT name FROM master.sys.server_principals WHERE name = 'user')
BEGIN
    CREATE LOGIN [user] WITH PASSWORD = 'root'
END
GO

USE [MusicDB]
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'user')
BEGIN
    CREATE USER [user] FOR LOGIN [user]
    EXEC sp_addrolemember N'db_datawriter', N'user'
    EXEC sp_addrolemember N'db_datareader', N'user'
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TRACK' and xtype='U')
BEGIN
    CREATE TABLE TRACK (
		track_id INT IDENTITY(1,1),
		title VARCHAR(30) NOT NULL,
		track_type INT,
		audio VARBINARY(MAX) NOT NULL,
		PRIMARY KEY(track_id),
		UNIQUE(title),
	);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SONG' and xtype='U')
BEGIN
    CREATE TABLE SONG (
		song_id INT IDENTITY(1,1),
		title VARCHAR(30) NOT NULL,
		artist VARCHAR(30) NOT NULL,
		audio VARBINARY(MAX) NOT NULL,
		PRIMARY KEY(song_id),
		UNIQUE(title),
	);
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MUSICTAG' and xtype='U')
BEGIN
    CREATE TABLE MUSICTAG (
		musictag_id INT IDENTITY(1,1),
		tag VARCHAR(30) NOT NULL,
		PRIMARY KEY(musictag_id),
		UNIQUE(tag),
	);
END
GO

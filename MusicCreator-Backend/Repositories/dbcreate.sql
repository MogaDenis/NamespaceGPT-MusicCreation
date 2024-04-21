IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MusicDB')
BEGIN
    CREATE DATABASE [MusicDB]
END

GO
    USE [MusicDB]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TRACK')
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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SONG')
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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MUSICTAG')
BEGIN
    CREATE TABLE MUSICTAG (
		musictag_id INT IDENTITY(1,1),
		tag VARCHAR(30) NOT NULL,
		PRIMARY KEY(musictag_id),
		UNIQUE(tag),
	);
END

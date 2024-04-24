// <copyright file="Track.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Music.MusicDomain
{
    using Plugin.Maui.Audio;

    /// <summary>
    ///     Track entity class.
    /// </summary>
    public class Track
    {
        private readonly IAudioManager audioManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="id">int. Id of Track.</param>
        /// <param name="title">string. Title of Track.</param>
        /// <param name="type">int. Type of Track.</param>
        /// <param name="songData">byte[]. SongData in the form of bytes.</param>
        public Track(int id, string title, int type, byte[] songData)
        {
            this.Id = id;
            this.Title = title;
            this.Type = type;
            this.SongData = songData;
            this.audioManager = AudioManager.Current;
        }

        /// <summary>
        ///     Gets the id of the Track.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the title of the Track.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Gets the type of the Track.
        /// </summary>
        public int Type { get; } // 1 - drums, 2 - instrument, 3 - fx, 4 - voice

        /// <summary>
        ///     Gets the SongData in the form of bytes.
        /// </summary>
        public byte[] SongData { get; }

        /// <summary>
        ///     Gets or sets the AudioPlayer.
        /// </summary>
        public IAudioPlayer? AudioPlayer { get; set; }

        /// <summary>
        ///     Plays the Track.
        /// </summary>
        public void Play()
        {
            if (this.SongData.Length == 0)
            {
                return;
            }

            this.AudioPlayer ??= this.audioManager.CreatePlayer(new MemoryStream(this.SongData));

            this.AudioPlayer.Stop();
            this.AudioPlayer.Loop = true;
            this.AudioPlayer.Play();
        }

        /// <summary>
        ///     Stops playing of the Track.
        /// </summary>
        public void Stop()
        {
            this.AudioPlayer?.Stop();
        }
    }
}
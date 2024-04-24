// <copyright file="Song.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Music.MusicDomain
{
    /// <summary>
    ///     Song entity class.
    /// </summary>
    public class Song : Track
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="id">int. Id of the Song.</param>
        /// <param name="title">string. Title of the Song.</param>
        /// <param name="type">int. Type of the Song.</param>
        /// <param name="audioData">byte[]. AudioData in the form of bytes.</param>
        /// <param name="artist">string. Name of the artist.</param>
        public Song(int id, string title, int type, byte[] audioData, string artist)
            : base(id, title, type, audioData)
        {
            this.Artist = artist;
        }

        /// <summary>
        ///     Gets the artist of the Song.
        /// </summary>
        public string Artist { get; }
    }
}

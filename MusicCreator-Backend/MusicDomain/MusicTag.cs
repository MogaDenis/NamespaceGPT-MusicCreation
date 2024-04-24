// <copyright file="MusicTag.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Music.MusicDomain
{
    /// <summary>
    ///     MusicTag entity class.
    /// </summary>
    public class MusicTag
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MusicTag"/> class.
        /// </summary>
        /// <param name="id">int. Id of the MusicTag.</param>
        /// <param name="title">string. Title of the MusicTag.</param>
        public MusicTag(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        ///     Gets or sets the id of the MusicTag.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the title of the MusicTag.
        /// </summary>
        public string Title { get; set; }
    }
}

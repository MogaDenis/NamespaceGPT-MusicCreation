// <copyright file="ICreationRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository.Interfaces
{
    using Music.MusicDomain;

    /// <summary>
    ///     Interface responsible for declaring functionality for manipulating the creation.
    /// </summary>
    public interface ICreationRepository
    {
        /// <summary>
        ///     Adds a Track to the creation.
        /// </summary>
        /// <param name="track">Track to add.</param>
        void AddTrack(Track track);

        /// <summary>
        ///     Removes a Track from the creation.
        /// </summary>
        /// <param name="id">The id of the Track to be removed.</param>
        void RemoveTrack(int id);

        /// <summary>
        ///     Removes a Track from the creation.
        /// </summary>
        /// <param name="track">The Track to be removed.</param>
        void RemoveTrack(Track track);

        /// <summary>
        ///     Returns all Tracks in the creation.
        /// </summary>
        /// <returns>List of Tracks.</returns>
        List<Track> GetTracks();

        /// <summary>
        ///     Returns the creation.
        /// </summary>
        /// <returns>Track representing the whole creation.</returns>
        Track GetCreation();

        /// <summary>
        ///     Plays the current creation.
        /// </summary>
        void PlayCreation();

        /// <summary>
        ///     Stops the playing of the current creation.
        /// </summary>
        void StopCreation();

        /// <summary>
        ///     Saves the creation.
        /// </summary>
        /// <param name="title">string. The title of the creation.</param>
        /// <returns>The creation as a Song object.</returns>
        Song SaveCreation(string title);
    }
}

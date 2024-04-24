// <copyright file="ITrackRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository.Interfaces
{
    using Music.MusicDomain;

    /// <summary>
    /// Interface for the TrackRepository.
    /// </summary>
    public interface ITrackRepository
    {
        /// <summary>
        /// Adds a new track to the database.
        /// </summary>
        /// <param name="newTrack">the new track that needs to be added.</param>
        /// <returns>the id of the new track.</returns>
        int Add(Track newTrack);

        /// <summary>
        /// deletes the element with the given id from the database.
        /// </summary>
        /// <param name="idToBeDeleted">the id of the element that needs to be deleted.</param>
        void Delete(int idToBeDeleted);

        /// <summary>
        /// Retrieves the track with the given id.
        /// </summary>
        /// <param name="idToSearch">the id the method searches for.</param>
        /// <returns>The track with the given id.</returns>
        Track? GetById(int idToSearch);

        /// <summary>
        /// Returns the list of all the tracks.
        /// </summary>
        /// <returns>A list of all the tracks.</returns>
        List<Track> GetAll();
    }
}

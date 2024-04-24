// <copyright file="ISongRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository.Interfaces
{
    using Music.MusicDomain;

    /// <summary>
    ///     Interface for declaring functionalities of a SongRepository.
    /// </summary>
    public interface ISongRepository
    {
        /// <summary>
        ///     Adds a Song to the repository.
        /// </summary>
        /// <param name="elem">Song to add.</param>
        /// <returns>The id of the new Song.</returns>
        int Add(Song elem);

        /// <summary>
        ///     Removes a Song from the repository.
        /// </summary>
        /// <param name="id">The id of the Song to be removed.</param>
        void Delete(int id);

        /// <summary>
        ///     Searches for the Song with the given id in the repository.
        /// </summary>
        /// <param name="id">The id of the Song.</param>
        /// <returns>The Song, if found, null otherwise.</returns>
        Song? Search(int id);

        /// <summary>
        ///     Returns all Songs in the repository.
        /// </summary>
        /// <returns>List of Songs.</returns>
        List<Song> GetAll();
    }
}

// <copyright file="IMusicTagRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository.Interfaces
{
    using Music.MusicDomain;

    /// <summary>
    ///     Interface responsible for declaring functionality for manipulating MusicTag entities.
    /// </summary>
    public interface IMusicTagRepository
    {
        /// <summary>
        ///     Method to add a MusicTag.
        /// </summary>
        /// <param name="elem">MusicTag to add.</param>
        /// <returns>The id of the new MusicTag.</returns>
        int Add(MusicTag elem);

        /// <summary>
        ///     Method to retrieve a MusicTag by id.
        /// </summary>
        /// <param name="id">Id of a MusicTag.</param>
        /// <returns>MusicTag object if found, null otherwise.</returns>
        MusicTag? Search(int id);

        /// <summary>
        ///     Method to retrieve all MusicTags.
        /// </summary>
        /// <returns>List of MusicTags.</returns>
        List<MusicTag> GetAll();
    }
}

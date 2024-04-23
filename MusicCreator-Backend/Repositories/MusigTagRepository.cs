// <copyright file="MusigTagRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;
  
    /// <summary>
    /// .
    /// </summary>
    public class MusigTagRepository : IMusicTagRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusigTagRepository"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory used to create database connections.</param>
        public MusigTagRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Adds a new music tag to the repository.
        /// </summary>
        /// <param name="elem">The music tag to add.</param>
        public int Add(MusicTag elem)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO MUSICTAG (tag) VALUES (@tag); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@tag", elem.Title);

            int newMusicTagId = Convert.ToInt32(command.ExecuteScalar());
            return newMusicTagId;
        }

        /// <summary>
        /// Searches for a music tag with the specified ID in the repository.
        /// </summary>
        /// <param name="id">The ID of the music tag to search for.</param>
        /// <returns>The music tag with the specified ID, or null if not found.</returns>
        public MusicTag? Search(int id)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM MUSICTAG WHERE musictag_id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                MusicTag musicTag = new (reader.GetInt32(0), reader.GetString(1));

                return musicTag;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all music tags from the repository.
        /// </summary>
        /// <returns>A list of all music tags in the repository.</returns>
        public List<MusicTag> GetAll()
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM MUSICTAG";

            SqlDataReader reader = command.ExecuteReader();
            List<MusicTag> musicTags = [];

            while (reader.Read())
            {
                MusicTag musicTag = new (reader.GetInt32(0), reader.GetString(1));

                musicTags.Add(musicTag);
            }

            return musicTags;
        }
    }
}
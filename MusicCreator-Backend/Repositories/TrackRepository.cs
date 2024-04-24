// <copyright file="TrackRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    /// <summary>
    /// A repository for the track object.
    /// </summary>
    public class TrackRepository : ITrackRepository
    {
        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackRepository"/> class.
        /// </summary>
        /// <param name="connectionFactory">The sql connection.</param>
        public TrackRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Adds a new track to the database.
        /// </summary>
        /// <param name="newTrack">the new track that needs to be added.</param>
        /// <returns>the id of the new track.</returns>
        public int Add(Track newTrack)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO TRACK (title, track_type, audio) VALUES (@title, @track_type, @audio); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@title", newTrack.Title);
            command.Parameters.AddWithValue("@track_type", newTrack.Type);
            command.Parameters.AddWithValue("@audio", newTrack.SongData);

            int newTrackId = Convert.ToInt32(command.ExecuteScalar());
            return newTrackId;
        }

        /// <summary>
        /// deletes the element with the given id from the database.
        /// </summary>
        /// <param name="idToBeDeleted">the id of the element that needs to be deleted.</param>
        public void Delete(int idToBeDeleted)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM TRACK WHERE track_id = @id";

            command.Parameters.AddWithValue("@id", idToBeDeleted);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves the track with the given id.
        /// </summary>
        /// <param name="idToSearch">the id the method searches for.</param>
        /// <returns>The track with the given id.</returns>
        public Track? GetById(int idToSearch)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM TRACK WHERE track_id = @id";

            command.Parameters.AddWithValue("@id", idToSearch);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                long byteLength = reader.GetBytes(3, 0, null, 0, 0);
                byte[] byteArray = new byte[byteLength];
                reader.GetBytes(3, 0, byteArray, 0, (int)byteLength);

                Track track = new (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), byteArray);

                return track;
            }

            return null;
        }

        /// <summary>
        /// Returns the list of all the tracks.
        /// </summary>
        /// <returns>A list of all the tracks.</returns>
        public List<Track> GetAll()
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM TRACK";

            SqlDataReader reader = command.ExecuteReader();
            List<Track> tracks = [];

            while (reader.Read())
            {
                long byteLength = reader.GetBytes(3, 0, null, 0, 0);
                byte[] byteArray = new byte[byteLength];
                reader.GetBytes(3, 0, byteArray, 0, (int)byteLength);

                Track track = new (reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), byteArray);

                tracks.Add(track);
            }

            return tracks;
        }
    }
}
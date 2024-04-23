namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    /// <summary>
    ///     Class responsible from getting and writing Song entities to the database.
    /// </summary>
    public class SongRepository : ISongRepository
    {
        private readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SongRepository"/> class.
        /// </summary>
        /// <param name="connectionFactory"> The connection factory used to create database connections.</param>
        public SongRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        ///     Adds a new song to the repository.
        /// </summary>
        /// <param name="elem">The song to add.</param>
        /// <returns>The id of the added song.</returns>
        public int Add(Song elem)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO SONG (title, artist, audio) VALUES (@title, @artist, @audio); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@title", elem.Title);
            command.Parameters.AddWithValue("@artist", elem.Artist);
            command.Parameters.AddWithValue("@audio", elem.SongData);

            int newSongId = Convert.ToInt32(command.ExecuteScalar());
            return newSongId;
        }

        /// <summary>
        ///     Deletes the song with ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the song to delete.</param>
        public void Delete(int id)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM SONG WHERE song_id = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Searches for a song with the specified ID in the repository.
        /// </summary>
        /// <param name="id">The ID of the song to search for.</param>
        /// <returns>Thesong with the specified ID, or null if not found.</returns>
        public Song? Search(int id)
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM SONG WHERE song_id = @id";

            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                long byteLength = reader.GetBytes(3, 0, null, 0, 0);
                byte[] byteArray = new byte[byteLength];
                reader.GetBytes(3, 0, byteArray, 0, (int)byteLength);

                Song song = new (reader.GetInt32(0), reader.GetString(1), 0, byteArray, reader.GetString(2));

                return song;
            }

            return null;
        }

        /// <summary>
        ///     Retrieves all songs from the repository.
        /// </summary>
        /// <returns>A list of all songs from the repository.</returns>
        public List<Song> GetAll()
        {
            using SqlConnection connection = this.connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM SONG";

            SqlDataReader reader = command.ExecuteReader();
            List<Song> songs = [];

            while (reader.Read())
            {
                long byteLength = reader.GetBytes(3, 0, null, 0, 0);
                byte[] byteArray = new byte[byteLength];
                reader.GetBytes(3, 0, byteArray, 0, (int)byteLength);

                Song song = new (reader.GetInt32(0), reader.GetString(1), 0, byteArray, reader.GetString(2));

                songs.Add(song);
            }

            return songs;
        }
    }
}

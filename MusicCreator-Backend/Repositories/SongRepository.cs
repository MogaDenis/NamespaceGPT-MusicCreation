namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    public class SongRepository : ISongRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public SongRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Add(Song elem)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
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

        public void Delete(int id)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM SONG WHERE song_id = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public Song? Search(int id)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
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

                Song song = new(reader.GetInt32(0), reader.GetString(1), 0, byteArray, reader.GetString(2));

                return song;
            }

            return null;
        }

        public List<Song> GetAll()
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
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

namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    public class TrackRepository : ITrackRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TrackRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Add(Track elem)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO TRACK (title, track_type, audio) VALUES (@title, @track_type, @audio); SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@title", elem.Title);
            command.Parameters.AddWithValue("@track_type", elem.Type);
            command.Parameters.AddWithValue("@audio", elem.SongData);

            int newTrackId = Convert.ToInt32(command.ExecuteScalar());
            return newTrackId;
        }

        public void Delete(int id)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "DELETE FROM TRACK WHERE track_id = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public Track? Search(int id)
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM TRACK WHERE track_id = @id";

            command.Parameters.AddWithValue("@id", id);

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

        public List<Track> GetAll()
        {
            using SqlConnection connection = this._connectionFactory.GetConnection();
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
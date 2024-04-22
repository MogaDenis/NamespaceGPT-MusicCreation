namespace MusicCreator.Repository
{
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    public class MusigTagRepository : IMusicTagRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public MusigTagRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

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
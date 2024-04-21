using System.Data;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlDataAdapter _adapter;
        private readonly DataSet _dataset;
        private readonly DataTable? _table;
        private readonly SqlCommandBuilder _commandBuilder;

        private readonly IConnectionFactory _connectionFactory;

        private static Song GenerateSongFromRowObject(DataRow row)
        {
            //int id = (int)row["song_id"]; // ...
            string title = (string)row["title"];
            string artist = (string)row["artist"];
            byte[] audio = (byte[])row["audio"];
            return new Song(0, title, 0, audio, artist);
        }

        public SongRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            string query = "select * from SONG";
            _connection = _connectionFactory.GetConnection();

            _adapter = new SqlDataAdapter(query, _connection);
            _dataset = new DataSet();
            _adapter.Fill(_dataset, "Song");
            _table = _dataset.Tables["Song"];

            _commandBuilder = new SqlCommandBuilder(_adapter);
            _adapter.InsertCommand = _commandBuilder.GetInsertCommand();
            _adapter.DeleteCommand = _commandBuilder.GetDeleteCommand();
        }

        public void Add(Song elem)
        {
            if (_table == null)
            {
                return;
            }

            DataRow row = _table.NewRow();
            row["title"] = elem.Title;
            row["artist"] = elem.Artist;
            row["audio"] = elem.SongData;
            _table.Rows.Add(row);
            _adapter.Update(_dataset, "Song");
        }

        public void Delete(Song elem)
        {
            if (_table == null)
            {
                return;
            }

            foreach (DataRow row in _table.Rows)
            {
                if ((int)row["song_id"] == elem.Id)
                    row.Delete();
            }
            _dataset.AcceptChanges();
            _adapter.Update(_dataset, "Song");
        }

        public Song? Search(int id)
        {
            if (_table == null)
            {
                return null;
            }

            var elems = from DataRow row in _table.Rows
                        where (int)row["song_id"] == id
                        select row;

            if (elems == null)
                return null;

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null)
            {
                return null;
            }

            return GenerateSongFromRowObject(elem);
        }

        public List<Song> GetAll()
        {
            if (_table == null)
            {
                return [];
            }

            var elems = from DataRow row in _table.Rows
                        select GenerateSongFromRowObject(row);
            return elems.ToList();
        }
    }
}

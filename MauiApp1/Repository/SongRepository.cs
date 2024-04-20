using System.Data;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Repository
{
    internal class SongRepository : ISongRepository
    {
        private SqlConnection _connection;
        private SqlDataAdapter _adapter;
        private DataSet _dataset;
        private DataTable? _table;
        private SqlCommandBuilder _commandBuilder;

        private static string GetConnectionString()
        {
            return "Data Source=192.168.1.140,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true";
        }

        private Song GenerateSongFromRowObject(DataRow row)
        {
            int id = (int)row["song_id"]; // ...
            string title = (string)row["title"];
            string artist = (string)row["artist"];
            byte[] audio = (byte[])row["audio"];
            return new Song(id, title, 0, audio, artist);
        }

        public SongRepository()
        {
            string query = "select * from SONG";
            _connection = new SqlConnection(GetConnectionString());

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

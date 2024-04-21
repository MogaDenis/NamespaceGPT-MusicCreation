using System.Data;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Repository
{
    public class TrackRepository : ITrackRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlDataAdapter _adapter;
        private readonly DataSet _dataset;
        private readonly DataTable? _table;
        private readonly SqlCommandBuilder _commandBuilder;

        private readonly IConnectionFactory _connectionFactory;

        private static Track GenerateTrackFromRowObject(DataRow row)
        {
            int id = (int)row["track_id"]; // ...
            string title = (string)row["title"];
            int type = (int)row["track_type"];
            byte[] audio = (byte[])row["audio"];
            return new Track(id, title, type, audio);
        }

        public TrackRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            // initializing connection
            string query = "select * from TRACK";
            _connection = _connectionFactory.GetConnection();

            // filling dataset
            _adapter = new SqlDataAdapter(query, _connection);
            _dataset = new DataSet();
            
            _adapter.Fill(_dataset, "Track");

            _table = _dataset.Tables["Track"];

            // building commands for the adapter
            _commandBuilder = new SqlCommandBuilder(_adapter);
            _adapter.InsertCommand = _commandBuilder.GetInsertCommand();
            _adapter.DeleteCommand = _commandBuilder.GetDeleteCommand();
        }

        public void Add(Track elem)
        {
            if (_table == null)
            {
                return;
            }

            DataRow row = _table.NewRow();
            row["title"] = elem.Title;
            row["track_type"] = elem.Type;
            row["audio"] = elem.SongData;
            _table.Rows.Add(row);
            _adapter.Update(_dataset, "Track");
        }

        public void Delete(Track elem)
        {
            if (_table == null)
            {
                return;
            }

            foreach (DataRow row in _table.Rows)
            {
                if ((int)row["track_id"] == elem.Id)
                    row.Delete();
            }

            _dataset.AcceptChanges();
            _adapter.Update(_dataset, "Track");
        }

        public Track? Search(int id)
        {
            if (_table == null)
            {
                return null;
            }

            var elems = from DataRow row in _table.Rows
                        where (int)row["track_id"] == id // yeah, trust me bro
                        select row;

            if (elems == null)
                return null;

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null)
            {
                return null;
            }

            return GenerateTrackFromRowObject(elem);
        }

        public List<Track> GetAll()
        {
            if (_table == null)
            {
                return [];
            }

            var elems = from DataRow row in _table.Rows
                        select GenerateTrackFromRowObject(row);

            return elems.ToList();
        }
    }
}
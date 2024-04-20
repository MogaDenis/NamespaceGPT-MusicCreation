using System.Data;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Repository
{
    internal class MusigTagRepository : IMusicTagRepository
    {
        private readonly SqlConnection _connection;
        private readonly SqlDataAdapter _adapter;
        private readonly DataSet _dataset;
        private readonly DataTable? _table;

        private static string GetConnectionString()
        {
            return "Data Source=192.168.1.140,2002;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False;Integrated Security=false;TrustServerCertificate=true";
        }

        private static MusicTag GenerateMusicTagFromRowObject(DataRow row)
        {
            int id = (int)row["musictag_id"]; // I hope you will do a better job
            string title = (string)row["tag"];
            return new MusicTag(id, title);
        }

        public MusigTagRepository() 
        {
            string query = "select * from MUSICTAG";
            _connection = new SqlConnection(GetConnectionString());

            _adapter = new SqlDataAdapter(query, _connection);
            _dataset = new DataSet();

            _adapter.Fill(_dataset, "MusicTag");
            _table = _dataset.Tables["MusicTag"]; 

            var commandBuilder = new SqlCommandBuilder(_adapter);
            _adapter.InsertCommand = commandBuilder.GetInsertCommand();
        }

        public void Add(MusicTag elem)
        {
            if (_table == null)
            {
                return;
            }

            DataRow row = _table.NewRow();
            //row["musictag_id"] = elem.getId();
            row["tag"] = elem.Title;
            _table.Rows.Add(row);
            _adapter.Update(_dataset, "MusicTag");
        }

        public MusicTag? Search(int id)
        {
            if (_table == null)
            {
                return null;
            }

            var elems = from DataRow row in _table.Rows
                        where (int)row["musictag_id"] == id
                        select row;

            if (elems == null)
                return null;

            DataRow? elem = elems.FirstOrDefault();
            if (elem == null)
            {
                return null;
            }

            return GenerateMusicTagFromRowObject(elem); 
        }

        public List<MusicTag> GetAll()
        {
            if (_table == null)
            {
                return [];
            }

            var elems = from DataRow row in _table.Rows
                        select GenerateMusicTagFromRowObject(row);
            return elems.ToList();
        }
    }
}

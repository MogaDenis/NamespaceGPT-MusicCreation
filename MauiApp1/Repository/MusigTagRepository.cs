using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using System.Text.RegularExpressions;

namespace MusicCreator.Repository
{
    internal class MusigTagRepository : IMusicTagRepository
    {
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private DataSet dataset;
        private DataTable? table;
        private string query;
        private SqlCommandBuilder cmdBuild;

        private string getConnectionString()
        {
            return "Data Source=192.168.43.73,1235;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False";
        }

        private MusicTag generateMusicTagFromRowObject(DataRow row)
        {
            int id = (int)row["musictag_id"]; // I hope you will do a better job
            string title = (string)row["tag"];
            return new MusicTag(id, title);
        }

        public MusigTagRepository() 
        {
            // initializing connection
            query = "select * from MUSICTAG";
            conn = new SqlConnection(getConnectionString());

            // filling dataset
            adapter = new SqlDataAdapter(query, conn);
            dataset = new DataSet();
            adapter.Fill(dataset, "MusicTag");
            table = dataset.Tables["MusicTag"]; // this should be a shallow copy

            // building commands for the adapter
            cmdBuild = new SqlCommandBuilder(adapter);
            adapter.InsertCommand = cmdBuild.GetInsertCommand();
        }

        public void add(MusicTag elem)
        {
            DataRow row = table.NewRow();
            //row["musictag_id"] = elem.getId();
            row["tag"] = elem.getTitle();
            table.Rows.Add(row);
            adapter.Update(dataset, "MusicTag");
        }

        public MusicTag? search(int id)
        {
            var elems = from DataRow row in table.Rows
                        where (int)row["musictag_id"] == id // yeah, trust me bro
                        select row;

            if (elems == null)
                return null;

            DataRow elem = elems.FirstOrDefault();
            return generateMusicTagFromRowObject(elem); 
        }

        public List<MusicTag> getAll()
        {
            var elems = from DataRow row in table.Rows
                        select generateMusicTagFromRowObject(row);
            return elems.ToList();
        }
    }
}

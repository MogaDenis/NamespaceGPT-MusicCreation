using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Music.MusicDomain;
using System.Text.RegularExpressions;

namespace MusicCreator.Repository
{
    internal class TrackRepository : ITrackRepository
    {
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private DataSet dataset;
        private DataTable? table;
        private string query;
        private SqlCommandBuilder cmdBuild;
        private List<Track> tracks;

        private string getConnectionString()
        {
            return "Data Source=192.168.43.73,1235;Initial Catalog=MusicDB;" +
                "User Id=user;Password=root;Encrypt=False";
        }

        private Track generateTrackFromRowObject(DataRow row)
        {
            int id = (int)row["track_id"]; // ...
            string title = (string)row["title"];
            int type = (int)row["track_type"];
            byte[] audio = (byte[])row["audio"];
            return new Track(id, title, type, audio);
        }

        public TrackRepository()
        {
            // initializing connection
            query = "select * from TRACK";
            conn = new SqlConnection(getConnectionString());

            // filling dataset
            adapter = new SqlDataAdapter(query, conn);
            dataset = new DataSet();
            adapter.Fill(dataset, "Track");
            table = dataset.Tables["Track"]; // this should be a shallow copy

            // building commands for the adapter
            cmdBuild = new SqlCommandBuilder(adapter);
            adapter.InsertCommand = cmdBuild.GetInsertCommand();
            adapter.DeleteCommand = cmdBuild.GetDeleteCommand();
        }

        public void add(Track elem)
        {
            DataRow row = table.NewRow();
            row["title"] = elem.getTitle();
            row["track_type"] = elem.getType();
            row["audio"] = elem.getSongData();
            table.Rows.Add(row);
            adapter.Update(dataset, "Track");
        }

        public void delete(Track elem)
        {
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["track_id"] == elem.getId())
                    row.Delete();
            }
            dataset.AcceptChanges();
            adapter.Update(dataset, "Track");
        }

        public Track? search(int id)
        {
            var elems = from DataRow row in table.Rows
                        where (int)row["track_id"] == id // yeah, trust me bro
                        select row;

            if (elems == null)
                return null;

            DataRow elem = elems.FirstOrDefault();
            return generateTrackFromRowObject(elem);
        }

        public List<Track> getAll()
        {
            if (tracks != null)
                return tracks;
            var elems = from DataRow row in table.Rows
                        select generateTrackFromRowObject(row);
            tracks = elems.ToList();
            return tracks;
        }
    }
}
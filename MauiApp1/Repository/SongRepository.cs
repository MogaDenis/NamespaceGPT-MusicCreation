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
    internal class SongRepository : ISongRepository
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

        private Song generateSongFromRowObject(DataRow row)
        {
            int id = (int)row["song_id"]; // ...
            string title = (string)row["title"];
            string artist = (string)row["artist"];
            byte[] audio = (byte[])row["audio"];
            return new Song(id, title, 0, audio, artist);
        }

        public SongRepository()
        {
            // initializing connection
            query = "select * from SONG";
            conn = new SqlConnection(getConnectionString());

            // filling dataset
            adapter = new SqlDataAdapter(query, conn);
            dataset = new DataSet();
            adapter.Fill(dataset, "Song");
            table = dataset.Tables["Song"];

            // building commands for the adapter
            cmdBuild = new SqlCommandBuilder(adapter);
            adapter.InsertCommand = cmdBuild.GetInsertCommand();
            adapter.DeleteCommand = cmdBuild.GetDeleteCommand();
        }

        public void add(Song elem)
        {
            DataRow row = table.NewRow();
            row["title"] = elem.getTitle();
            row["artist"] = elem.getArtist();
            row["audio"] = elem.getSongData();
            table.Rows.Add(row);
            adapter.Update(dataset, "Song");
        }

        public void delete(Song elem)
        {
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["song_id"] == elem.getId())
                    row.Delete();
            }
            dataset.AcceptChanges();
            adapter.Update(dataset, "Song");
        }

        public Song? search(int id)
        {
            var elems = from DataRow row in table.Rows
                        where (int)row["song_id"] == id // yeah, trust me bro
                        select row;

            if (elems == null)
                return null;

            DataRow elem = elems.FirstOrDefault();
            return generateSongFromRowObject(elem);
        }

        public List<Song> getAll()
        {
            var elems = from DataRow row in table.Rows
                        select generateSongFromRowObject(row);
            return elems.ToList();
        }
    }
}

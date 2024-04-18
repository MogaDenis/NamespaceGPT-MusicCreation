using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using NAudio.Wave;
using System.Text.Json.Serialization;

//=====================================

Console.WriteLine(Environment.CurrentDirectory);
Console.WriteLine();

Console.WriteLine("Give your IP address (be responsible, no validation): ");
string ip = Console.ReadLine();

Console.WriteLine("1. Test connection");
Console.WriteLine("2. Test connection and populate database");
int option = Convert.ToInt32(Console.ReadLine());

TrackRepository repo = new TrackRepository(ip);
if (option == 2)
{
    List<Track> tracks = TrackCreator.CreateTracks(Environment.CurrentDirectory + "\\iss_loops_wav");
    foreach (Track track in tracks)
    {
        Console.WriteLine(track.getTitle());
        repo.add(track);
    }
}
else if (option != 1)
    Console.WriteLine("You're dumb");
else
    Console.WriteLine("No errors");

public class Track
{
    private int id;
    private string title;
    private int type; // 1 - drums, 2 - instrument, 3 - fx, 4 - voice
    private long timestamp = 0;
    private byte[] songData;
    private WaveOutEvent waveOut;
    private WaveStream waveStream;
    private bool playing = false;

    public Track(int id, string title, int type, byte[] songData)
    {
        this.id = id;
        this.title = title;
        this.type = type;
        this.songData = songData;
        waveOut = new WaveOutEvent();
    }
    public int getId()
    {
        return id;
    }

    public byte[] getSongData()
    {
        return songData;
    }

    public string getTitle()
    {
        return title;
    }

    public int getType()
    {
        return type;
    }


    public void Play()
    {
        if (!playing)
        {
            //read the wav data from the byte array
            waveStream = new WaveFileReader(new MemoryStream(songData));
            waveOut.Dispose();
            waveOut.Init(waveStream);
            waveOut.Play();
            playing = true;
        }
    }

    public void Pause()
    {
        if (playing)
        {
            timestamp = waveOut.GetPosition();
            waveOut.Pause();
            playing = false;
        }
    }

    public void Stop()
    {
        if (playing)
        {
            playing = false;
            timestamp = 0;
            waveOut.Dispose();
            waveStream.Dispose();
        }

    }

    public void Resume()
    {
        if (!playing)
        {
            WaveStream waveStream = new WaveFileReader(new MemoryStream(songData));
            waveStream.Seek(timestamp, SeekOrigin.Begin);
            waveOut.Dispose();
            waveOut.Init(waveStream);
            waveOut.Play();
            playing = true;
        }
    }

    ~Track()
    {
        waveOut.Dispose();
    }
}

internal class TrackCreator
{
    public static List<Track> CreateTracks(string folderpath)
    {
        List<Track> tracks = new List<Track>();
        string[] files = Directory.GetFiles(folderpath);
        foreach (string fileName in files)
        {
            byte[] wavData = File.ReadAllBytes(fileName);
        
            string[] nameElements = fileName.Split(new char[] { '\\' });
            string name = nameElements[nameElements.Length - 1];
            name = name.Split(new char[] { '.' })[0];
            nameElements = name.Split(new char[] { '_' });
            name = nameElements.Aggregate((a, b) => a + " " + b);
            name = char.ToUpper(name[0]) + name.Substring(1);

            int trackType = 0;
            if (fileName.Contains("drums"))
            {
                trackType = 1;
            }
            if (fileName.Contains("melody"))
            {
                trackType = 2;
            }
            if (fileName.Contains("fx"))
            {
                trackType = 3;
            }
            if (fileName.Contains("voice"))
            {
                trackType = 4;
            }
            tracks.Add(new Track(1, name, trackType, wavData));
        }
        return tracks;
    }
}

internal class TrackRepository
{
    private SqlConnection conn;
    private SqlDataAdapter adapter;
    private DataSet dataset;
    private DataTable? table;
    private string query;
    private SqlCommandBuilder cmdBuild;
    private string ip;

    private string getConnectionString()
    {
        return "Data Source=" + ip + ",1235;" +
            "Integrated Security=true;Encrypt=False";
    }

    private string getConnectionString2()
    {
        return "Data Source=" + ip + ",1235;Initial Catalog=MusicDB;" +
            "Integrated Security=true;Encrypt=False";
    }

    private Track generateTrackFromRowObject(DataRow row)
    {
        int id = (int)row["track_id"]; // ...
        string title = (string)row["title"];
        int type = (int)row["track_type"];
        byte[] audio = (byte[])row["audio"];
        return new Track(id, title, type, audio);
    }

    public TrackRepository(string ip)
    {
        this.ip = ip;

        // initializing connection
        conn = new SqlConnection(getConnectionString());
        query = "select * from TRACK";

        // creating database and tables if they do not exist already (from script)
        conn.Open();
        FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + "\\dbcreate.sql");
        string script = fileInfo.OpenText().ReadToEnd();
        Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        string[] lines = regex.Split(script);
        SqlCommand cmd = conn.CreateCommand();
        cmd.Connection = conn;
        foreach (string line in lines)
        {
            cmd.CommandText = line;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
        }
        conn.Close();

        // reconnecting to the database with another connection string
        // there might be a cleaner way; I didn't find it, good luck to you
        conn = new SqlConnection(getConnectionString2());

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
        var elems = from DataRow row in table.Rows
                    select generateTrackFromRowObject(row);
        return elems.ToList();
    }
}


namespace Music.MusicDomain
{
    public class Song : Track
    {
        public string Artist { get; }

        public Song(int id, string title, int type, byte[] audioData, string artist)
            : base(id, title, type, audioData)
        {
            Artist = artist;
        }
    }
}

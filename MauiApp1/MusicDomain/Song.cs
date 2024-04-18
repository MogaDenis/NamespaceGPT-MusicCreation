namespace Music.MusicDomain
{
    internal class Song : Track
    {
        private string artist;

        public Song(int id, string title, int type, byte[] audioData, string artist)
            : base(id, title, type, audioData)
        {
            this.artist = artist;
        }

        public string getArtist()
        {
            return artist;
        }
    }
}

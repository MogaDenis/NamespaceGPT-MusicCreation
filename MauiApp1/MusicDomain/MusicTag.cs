namespace Music.MusicDomain
{
    public class MusicTag
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public MusicTag(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}

namespace Music.MusicDomain
{
    internal class MusicTag
    {
        private int id;
        private string title;

        public MusicTag(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public string getTitle()
        {
            return title;
        }

        public int getId()
        {
            return id;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public void setId(int id)
        {
            this.id = id;
        }
    }
}

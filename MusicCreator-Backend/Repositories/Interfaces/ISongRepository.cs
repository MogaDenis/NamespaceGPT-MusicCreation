using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    public interface ISongRepository
    {
        void Add(Song elem);
        void Delete(Song elem);
        Song? Search(int id);
        List<Song> GetAll();
    }
}

using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    public interface ISongRepository
    {
        int Add(Song elem);
        void Delete(int id);
        Song? Search(int id);
        List<Song> GetAll();
    }
}

using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    public interface ITrackRepository
    {
        int Add(Track elem);
        void Delete(int id);
        Track? Search(int id);
        List<Track> GetAll();
    }
}

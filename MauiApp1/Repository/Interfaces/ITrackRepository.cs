using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    internal interface ITrackRepository
    {
        void Add(Track elem);
        void Delete(Track elem);
        Track? Search(int id);
        List<Track> GetAll();
    }
}

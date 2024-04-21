using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    public interface IMusicTagRepository
    {
        void Add(MusicTag elem);
        MusicTag? Search(int id);
        List<MusicTag> GetAll();
    }
}

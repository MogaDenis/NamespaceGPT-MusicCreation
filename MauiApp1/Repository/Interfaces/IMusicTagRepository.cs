using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    internal interface IMusicTagRepository
    {
        void Add(MusicTag elem);
        MusicTag? Search(int id);
        List<MusicTag> GetAll();
    }
}

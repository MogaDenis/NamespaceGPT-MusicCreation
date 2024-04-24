using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    public interface ICreationRepository
    {
        void AddTrack(Track track);
        void RemoveTrack(int id);
        void RemoveTrack(Track track);
        List<Track> GetTracks();
        Track GetCreation();
        void PlayCreation();
        void StopCreation();
        Song SaveCreation(string title);
    }
}

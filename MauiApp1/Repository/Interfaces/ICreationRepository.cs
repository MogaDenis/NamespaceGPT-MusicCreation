using Music.MusicDomain;

namespace MusicCreator.Repository.Interfaces
{
    internal interface ICreationRepository
    {
        void AddTrack(Track track);
        void RemoveTrack(int id);
        void RemoveTrack(Track track);
        List<Track> GetTracks();
        void PlayCreation();
        void StopCreation();
        Song SaveCreation(string title);
    }
}

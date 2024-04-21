using MusicCreator.Repository;
using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;

namespace MusicCreator.Services
{
    public class Service
    {
        private readonly ITrackRepository _trackRepository;
        private readonly ICreationRepository _creationRepository;
        private readonly ISongRepository _songRepository;
        public string Category { get; set; } = null!;

        private static Service? _instance = null;

        public static Service GetService()
        {
            if (_instance == null)
            {
                _instance = new Service();
            }

            return _instance;
        }

        private Service()
        {
            _trackRepository = new TrackRepository(new SqlConnectionFactory());
            _creationRepository = new CreationRepository();
             _songRepository = new SongRepository(new SqlConnectionFactory());
        }

        public List<Track> GetTracks()
        {
            return _trackRepository.GetAll();
        }

        public List<Track> GetTracksByType(int type) // 1 = Drum, 2 = Instrument, 3 = Fx, 4 = Voice
        {
            return _trackRepository.GetAll().FindAll(x => x.Type == type);
        }

        public Track? GetTrackById(int id)
        {
            return _trackRepository.GetAll().Find(x => x.Id == id);
        }

        public List<Track> GetCreationTracks()
        {
            return _creationRepository.GetTracks();
        }

        public void AddTrack(Track track)
        {
            _creationRepository.AddTrack(track);
        }

        public void AddTrack(int id)
        {
            var track = _trackRepository.GetAll().Find(t => t.Id == id);
            if (track == null)
            {
                return;
            }

            _creationRepository.AddTrack(track);
        }

        public void RemoveTrack(int id)
        {
            _creationRepository.RemoveTrack(id);
        }

        public void RemoveTrack(Track track)
        {
            _creationRepository.RemoveTrack(track);
        }

        public void PlayCreation()
        {
            _creationRepository.PlayCreation();
        }

        public void StopCreation()
        {
            _creationRepository.StopCreation();
        }

        public void SaveCreation(string title)
        {
            _songRepository.Add(_creationRepository.SaveCreation(title));
        }

        public void StopAll()
        {
            foreach (Track track in _trackRepository.GetAll())
            {
                track.Stop();
            }
        }
    }
}

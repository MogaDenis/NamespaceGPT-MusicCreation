using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCreator.Repository;
using Music.MusicDomain;

namespace MusicCreator.Services
{
    internal class Service
    {
        private TrackRepository _trackRepository;
        private CreationRepository _creationRepository;
        private SongRepository _songRepository;
        public string category { get; set;}

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
             _trackRepository = new TrackRepository();
             _creationRepository = new CreationRepository();
             _songRepository = new SongRepository();
        }

        public List<Track> GetTracks()
        {
            return _trackRepository.getAll();
        }

        public List<Track> GetTracksByType(int type) // 1 = Drum, 2 = Instrument, 3 = Fx, 4 = Voice
        {
            return _trackRepository.getAll().FindAll(x => x.getType() == type);
        }

        public Track GetTrackById(int id)
        {
            return _trackRepository.getAll().Find(x => x.getId() == id);
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
            _creationRepository.AddTrack(_trackRepository.getAll().Find(t => t.getId() == id));
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
            _creationRepository.playCreation();
        }

        public void StopCreation()
        {
            _creationRepository.stopCreation();
        }

        public void SaveCreation(string title)
        {
            _songRepository.add(_creationRepository.saveCreation(title));
        }

        public void StopAll()
        {
            foreach (Track track in _trackRepository.getAll())
            {
                track.Stop();
            }
        }
    }
}

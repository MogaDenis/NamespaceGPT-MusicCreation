using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.MusicDomain;

namespace MusicCreator.Services
{
    internal interface ICreationRepository
    {
        void AddTrack(Track track);
        void RemoveTrack(int id);
        void RemoveTrack(Track track);
        List<Track> GetTracks();
        void playCreation();
        void stopCreation();
        Song saveCreation(string title);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.MusicDomain;

namespace MusicCreator.Repository
{
    internal interface ISongRepository
    {
        void add(Song elem);
        void delete(Song elem);
        Song? search(int id);
        List<Song> getAll();
    }
}

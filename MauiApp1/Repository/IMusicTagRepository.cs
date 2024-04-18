using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.MusicDomain;

namespace MusicCreator.Repository
{
    internal interface IMusicTagRepository
    {
        void add(MusicTag elem);
        MusicTag? search(int id);
        List<MusicTag> getAll();
    }
}

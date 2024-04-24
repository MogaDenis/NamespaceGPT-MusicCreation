using Music.MusicDomain;
using MusicCreator.Repository.Interfaces;
using NAudio.Wave;

namespace MusicCreator.Repository
{
    public class CreationRepository : ICreationRepository
    {
        private readonly List<Track> tracks = [];
        private Track creation;
        private WaveMixerStream32 mixer;
        private readonly IConnectionFactory connectionFactory;
        // creation file is the "work in progress" file

        public CreationRepository()
        {
            mixer = new WaveMixerStream32();
            creation = new Track(0507, "creation", 1704, []);
            mixer.Dispose();
            // this is to empty the creation file
        }

        public CreationRepository(IConnectionFactory connectionFactory)
        {
            mixer = new WaveMixerStream32();
            creation = new Track(0507, "creation", 1704, []);
            mixer.Dispose();
            // this is to empty the creation file
            this.connectionFactory = connectionFactory;
        }
        public void AddTrack(Track track)
        {
            tracks.Add(track);
            GenerateCreation();
        }

        public void RemoveTrack(int id)
        {
            var track = tracks.Find(currentTrack => currentTrack.Id == id);
            if (track == null)
            {
                return;
            }

            tracks.Remove(track);
            GenerateCreation();
        }

        public void RemoveTrack(Track track)
        {
            tracks.Remove(track);
            GenerateCreation();
        }

        public List<Track> GetTracks()
        {
            return tracks;
        }

        public Track GetCreation()
        {
            return creation;
        }

        private void GenerateCreation()
        {
            mixer = new WaveMixerStream32();
            foreach (Track track in tracks)
            {
                WaveStream audio = new WaveFileReader(new MemoryStream(track.SongData));
                WaveChannel32 channel = new(audio);
                mixer.AddInputStream(channel);
            }

            creation.Stop();

            using MemoryStream memoryStream = new MemoryStream();
            // Buffer size for reading from the mixer
            byte[] buffer = new byte[4096];
            int bytesRead;

            // Reset the position of the mixer stream
            mixer.Position = 0;

            // Create a WaveFileWriter with the memory stream
            using (var waveWriter = new WaveFileWriter(memoryStream, mixer.WaveFormat))
            {
                // Read from the mixer and write to the memory stream until all data is read
                while ((bytesRead = mixer.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveWriter.Write(buffer, 0, bytesRead);
                }
            }

            // Convert the memory stream to a byte array
            byte[] audioBytes = memoryStream.ToArray();

            creation = new Track(0507, "creation", 1704, audioBytes);

            // Now you can use the audioBytes byte array as needed
        }

        public void PlayCreation()
        {
            //if (creation.GetPlaybackState() == PlaybackState.Stopped)
            creation.Play();
            //else if (creation.GetPlaybackState() == PlaybackState.Playing)
            //    creation.Stop();
        }

        public void StopCreation()
        {
            creation.Stop();
        }

        public Song SaveCreation(string title)
        {
            return new Song(123, title, 0, creation.SongData, "dummy");

            /*string outputPath = creationPath + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".wav";
            WaveMixerStream32 mixer = new WaveMixerStream32();
            WaveStream audio = new WaveFileReader(creation.getPath());
            WaveChannel32 channel = new WaveChannel32(audio);
            mixer.AddInputStream(channel);
            WaveFileWriter.CreateWaveFile(outputPath, mixer);*/
        }
    }
}

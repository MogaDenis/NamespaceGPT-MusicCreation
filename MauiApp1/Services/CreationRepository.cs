using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music.MusicDomain;
using NAudio.Wave;

namespace MusicCreator.Services
{
    internal class CreationRepository : ICreationRepository
    {
        private List<Track> tracks = new List<Track>();
        private Track creation;
        private WaveMixerStream32 mixer;
        // creation file is the "work in progress" file

        public CreationRepository()
        {
            mixer = new WaveMixerStream32();
            creation = new Track(0507, "creation", 1704, []);
            mixer.Dispose();
            // this is to empty the creation file
        }

        public void AddTrack(Track track)
        {
            tracks.Add(track);
            generateCreation();
        }

        public void RemoveTrack(int id)
        {
            tracks.Remove(tracks.Find(x => x.getId() == id));
            generateCreation();
        }

        public void RemoveTrack(Track track)
        {
            tracks.Remove(track);
            generateCreation();
        }

        public List<Track> GetTracks()
        {
            return tracks;
        }

        private void generateCreation()
        {
            mixer = new WaveMixerStream32();
            foreach (Track track in tracks)
            {
                WaveStream audio = new WaveFileReader(new MemoryStream(track.getSongData()));
                WaveChannel32 channel = new WaveChannel32(audio);
                mixer.AddInputStream(channel);
            }
            this.creation.Stop();

            using (MemoryStream memoryStream = new MemoryStream())
            {
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

                this.creation = new Track(0507, "creation", 1704, audioBytes);

                // Now you can use the audioBytes byte array as needed
            }


        }

        public void playCreation()
        {
            //if (creation.GetPlaybackState() == PlaybackState.Stopped)
                creation.Play();
            //else if (creation.GetPlaybackState() == PlaybackState.Playing)
            //    creation.Stop();

        }

        public void stopCreation()
        {
            creation.Stop();
        }



        public Song saveCreation(string title)
        {
            return new Song(123, title, 0, creation.getSongData(), "dummy");

            /*string outputPath = creationPath + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".wav";
            WaveMixerStream32 mixer = new WaveMixerStream32();
            WaveStream audio = new WaveFileReader(creation.getPath());
            WaveChannel32 channel = new WaveChannel32(audio);
            mixer.AddInputStream(channel);
            WaveFileWriter.CreateWaveFile(outputPath, mixer);*/
        }


    }
}

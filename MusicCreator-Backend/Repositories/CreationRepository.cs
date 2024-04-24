// <copyright file="CreationRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Repository
{
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;
    using NAudio.Wave;

    /// <summary>
    ///     Class responsible for managing the current creation.
    /// </summary>
    public class CreationRepository : ICreationRepository
    {
        private List<Track> tracks = [];
        private Track creation;
        private WaveMixerStream32 mixer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreationRepository"/> class.
        /// </summary>
        public CreationRepository()
        {
            this.mixer = new WaveMixerStream32();
            this.creation = new Track(0507, "creation", 1704, []);
            this.mixer.Dispose();
        }

        /// <summary>
        ///     Adds a Track to the creation.
        /// </summary>
        /// <param name="track">Track to add.</param>
        public void AddTrack(Track track)
        {
            this.tracks.Add(track);
            this.GenerateCreation();
        }

        /// <summary>
        ///     Removes a Track from the creation.
        /// </summary>
        /// <param name="id">The id of the Track to be removed.</param>
        public void RemoveTrack(int id)
        {
            this.tracks = this.tracks.Where(currentTrack => currentTrack.Id != id).ToList();
            this.GenerateCreation();
        }

        /// <summary>
        ///     Removes a Track from the creation.
        /// </summary>
        /// <param name="track">The Track to be removed.</param>
        public void RemoveTrack(Track track)
        {
            this.tracks.Remove(track);
            this.GenerateCreation();
        }

        /// <summary>
        ///     Returns all Tracks in the creation.
        /// </summary>
        /// <returns>List of Tracks.</returns>
        public List<Track> GetTracks()
        {
            return this.tracks;
        }

        /// <summary>
        ///     Returns the creation.
        /// </summary>
        /// <returns>Track representing the whole creation.</returns>
        public Track GetCreation()
        {
            return this.creation;
        }

        /// <summary>
        ///     Plays the current creation.
        /// </summary>
        public void PlayCreation()
        {
            this.creation.Play();
        }

        /// <summary>
        ///     Stops the playing of the current creation.
        /// </summary>
        public void StopCreation()
        {
            this.creation.Stop();
        }

        /// <summary>
        ///     Saves the creation.
        /// </summary>
        /// <param name="title">string. The title of the creation.</param>
        /// <returns>The creation as a Song object.</returns>
        public Song SaveCreation(string title)
        {
            return new Song(123, title, 0, this.creation.SongData, "dummy");
        }

        private void GenerateCreation()
        {
            this.mixer = new WaveMixerStream32();
            foreach (Track track in this.tracks)
            {
                try
                {
                    this.mixer.AddInputStream(new WaveChannel32(new WaveFileReader(new MemoryStream(track.SongData))));
                }
                catch (Exception)
                {
                }
            }

            this.creation.Stop();

            using MemoryStream memoryStream = new ();

            byte[] buffer = new byte[4096];
            int bytesRead;

            this.mixer.Position = 0;

            using (var waveWriter = new WaveFileWriter(memoryStream, this.mixer.WaveFormat))
            {
                while ((bytesRead = this.mixer.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveWriter.Write(buffer, 0, bytesRead);
                }
            }

            byte[] audioBytes = memoryStream.ToArray();

            this.creation = new Track(0507, "creation", 1704, audioBytes);
        }
    }
}

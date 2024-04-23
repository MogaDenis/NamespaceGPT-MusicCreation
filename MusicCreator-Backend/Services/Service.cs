// <copyright file="Service.cs" company="NamespaceGPT">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator.Services
{
    using Music.MusicDomain;
    using MusicCreator.Repository.Interfaces;

    /// <summary>
    ///     Class responsible for encapsulating the repositories.
    ///     This class uses the singleton pattern.
    /// </summary>
    public class Service
    {
        private static Service? instance = null;

        private readonly ITrackRepository trackRepository;
        private readonly ICreationRepository creationRepository;
        private readonly ISongRepository songRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class
        ///     and sets the static member to this new instance.
        /// </summary>
        /// <param name="trackRepository">ITrackRepository.</param>
        /// <param name="creationRepository">ICreationRepository.</param>
        /// <param name="songRepository">ISongRepository.</param>
        public Service(ITrackRepository trackRepository, ICreationRepository creationRepository, ISongRepository songRepository)
        {
            this.trackRepository = trackRepository;
            this.creationRepository = creationRepository;
            this.songRepository = songRepository;

            instance = this;
        }

        /// <summary>
        ///     Gets or sets property set to the category of the tracks shown.
        /// </summary>
        public string Category { get; set; } = null!;

        /// <summary>
        ///     Method which clears the static instance.
        /// </summary>
        public static void ResetInstance()
        {
            instance = null;
        }

        /// <summary>
        ///     Method which returns the unique instance of Service.
        /// </summary>
        /// <returns>The instance of the Service.</returns>
        public static Service GetService()
        {
            if (instance == null)
            {
                throw new NullReferenceException("You must initialize the Service class.");
            }

            return instance;
        }

        /// <summary>
        ///     Method which returns all tracks in the TrackRepository.
        /// </summary>
        /// <returns>List of Track objects.</returns>
        public List<Track> GetTracks()
        {
            return this.trackRepository.GetAll();
        }

        /// <summary>
        ///     Method which returns all tracks of the given type in the TrackRepository.
        /// </summary>
        /// <param name="type">int.</param>
        /// <returns>List of Track objects.</returns>
        public List<Track> GetTracksByType(int type) // 1 = Drum, 2 = Instrument, 3 = Fx, 4 = Voice
        {
            return this.trackRepository.GetAll().FindAll(currentTrack => currentTrack.Type == type);
        }

        /// <summary>
        ///     Method which returns the Track in the TrackRepository with the given id, or null if not found.
        /// </summary>
        /// <param name="id">int.</param>
        /// <returns>Track or null.</returns>
        public Track? GetTrackById(int id)
        {
            return this.trackRepository.GetAll().Find(currentTrack => currentTrack.Id == id);
        }

        /// <summary>
        ///     Method which returns the Track in the TrackRepository with the given title, or null if not found.
        /// </summary>
        /// <param name="title">string.</param>
        /// <returns>Track or null.</returns>
        public Track? GetTrackByTitle(string title)
        {
            return this.trackRepository.GetAll().Find(currentTrack => currentTrack.Title == title);
        }

        /// <summary>
        ///     Method which returns all tracks of the given type and having the given title in the TrackRepository.
        /// </summary>
        /// <param name="type">int.</param>
        /// <param name="searchQuery">string.</param>
        /// <returns>List of Track objects.</returns>
        public List<Track> GetTracksByTypeAndFilterByTitle(int type, string searchQuery)
        {
            return this.GetTracksByType(type)
                .FindAll(track => track.Title.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        ///     Method which returns all Track objects included in the current creation.
        /// </summary>
        /// <returns>List of Track objects.</returns>
        public List<Track> GetCreationTracks()
        {
            return this.creationRepository.GetTracks();
        }

        /// <summary>
        ///     Method which adds a Track to the current creation.
        /// </summary>
        /// <param name="track">Track.</param>
        public void AddTrack(Track track)
        {
            this.creationRepository.AddTrack(track);
        }

        /// <summary>
        ///     Method which adds a Track by searching it by id in the TrackRepository first. Only added if found.
        /// </summary>
        /// <param name="id">int.</param>
        public void AddTrack(int id)
        {
            var track = this.trackRepository.GetAll().Find(currentTrack => currentTrack.Id == id);
            if (track == null)
            {
                return;
            }

            this.creationRepository.AddTrack(track);
        }

        /// <summary>
        ///     Method which removes a Track by id from the current creation.
        /// </summary>
        /// <param name="id">int.</param>
        public void RemoveTrack(int id)
        {
            this.creationRepository.RemoveTrack(id);
        }

        /// <summary>
        ///     Method which removes a Track from the current creation.
        /// </summary>
        /// <param name="track">Track.</param>
        public void RemoveTrack(Track track)
        {
            this.creationRepository.RemoveTrack(track);
        }

        /// <summary>
        ///     Method which triggers the play functionality for the current creation.
        /// </summary>
        public void PlayCreation()
        {
            this.creationRepository.PlayCreation();
        }

        /// <summary>
        ///     Method which stops the play functionality for the current creation.
        /// </summary>
        public void StopCreation()
        {
            this.creationRepository.StopCreation();
        }

        /// <summary>
        ///     Method which saves the current creation to the SongRepository.
        /// </summary>
        /// <param name="title">string.</param>
        public void SaveCreation(string title)
        {
            this.songRepository.Add(this.creationRepository.SaveCreation(title));
        }

        /// <summary>
        ///     Method which stops all playing tracks.
        /// </summary>
        public void StopAll()
        {
            foreach (Track track in this.trackRepository.GetAll())
            {
                track.Stop();
            }
        }
    }
}

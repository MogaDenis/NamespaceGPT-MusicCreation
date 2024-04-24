// <copyright file="SearchPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using Music.MusicDomain;
    using MusicCreator.Services;

    /// <summary>
    ///     SearchPage ContentPage.
    /// </summary>
    public partial class SearchPage : ContentPage
    {
        private readonly Service service;
        private readonly int categoryAsInt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchPage"/> class.
        /// </summary>
        public SearchPage()
        {
            this.InitializeComponent();

            this.service = Service.GetService();

            string category = this.service.Category;

            if (category == "drums")
            {
                this.categoryAsInt = 1;
            }
            else if (category == "music")
            {
                this.categoryAsInt = 2;
            }
            else if (category == "fx")
            {
                this.categoryAsInt = 3;
            }
            else if (category == "mic")
            {
                this.categoryAsInt = 4;
            }
            else
            {
                this.categoryAsInt = 0;
            }

            var tracksData = this.service.GetTracksByType(this.categoryAsInt);
            this.TracksListView.ItemsSource = tracksData;
        }

        /// <summary>
        ///     EventHandler for OnTrackTapped Event.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">EventArguments.</param>
        public void OnTrackTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is not Track track)
            {
                return;
            }

            this.service.AddTrack(track);
            this.service.StopAll();

            Shell.Current.GoToAsync("Main");
        }

        /// <summary>
        ///     EventHandler for OnPlay Event.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">EventArguments.</param>
        public void OnPlayClicked(object sender, EventArgs e)
        {
            this.service.StopAll();
            int id = (int)((Button)sender).CommandParameter;
            var track = this.service.GetTrackById(id);
            if (track == null)
            {
                return;
            }

            track.Play();
        }

        /// <summary>
        ///     EventHandler for GoFromSearchToMainPage Event.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">EventArguments.</param>
        public async void GoFromSearchToMainPage(object sender, EventArgs e)
        {
            this.service.StopAll();
            await Shell.Current.GoToAsync("Main");
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string searchQuery = this.SearchBar.Text;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                this.TracksListView.ItemsSource = this.service.GetTracksByType(this.categoryAsInt);
            }
            else
            {
                this.TracksListView.ItemsSource = this.service
                    .GetTracksByTypeAndFilterByTitle(this.categoryAsInt, searchQuery);
            }
        }
    }
}
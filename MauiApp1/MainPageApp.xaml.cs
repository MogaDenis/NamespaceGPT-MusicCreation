// <copyright file="MainPageApp.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using Music.MusicDomain;
    using MusicCreator.Services;

    /// <summary>
    ///     MainPageApp ContentPage.
    /// </summary>
    public partial class MainPageApp : ContentPage
    {
        private readonly Service service;
        private readonly List<Track> tracks;
        private bool isButtonClicked;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPageApp"/> class.
        /// </summary>
        public MainPageApp()
        {
            this.InitializeComponent();
            this.service = Service.GetService();
            this.tracks = this.service.GetCreationTracks();
            List<string> items = (from t in this.tracks
                                  select t.Title).ToList();

            this.tracksListView.ItemsSource = items;
            this.isButtonClicked = false;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button { CommandParameter: string item } && this.tracksListView.ItemsSource is List<string> items)
            {
                items.Remove(item);
                var track = this.service.GetTrackByTitle(item);
                if (track == null)
                {
                    return;
                }

                int trackId = track.Id;
                this.service.RemoveTrack(trackId);
                this.tracksListView.ItemsSource = null;
                this.tracksListView.ItemsSource = items;
            }
        }

        private async void GoFromMainToLogInPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LogIn");
        }

        private void GoToListenTrack(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void GoToSearchTracks(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string category = button.Text.ToLower();
            this.service.Category = category;
            await Shell.Current.GoToAsync("Search");
        }

        private async void GoFromMainToSavePage(object sender, EventArgs e)
        {
            if (this.service.GetCreationTracks().Count == 0)
            {
                await this.DisplayAlert("Empty creation!", "Please select at least one track!", "OK");
                return;
            }

            await Shell.Current.GoToAsync("Save");
        }

        private void PlayCreation(object sender, EventArgs e)
        {
            if (!this.isButtonClicked && this.service.GetCreationTracks().Count != 0)
            {
                this.playButton.BackgroundColor = Color.FromRgb(255, 0, 0);
                this.isButtonClicked = true;
                this.service.PlayCreation();
            }
            else
            {
                this.playButton.BackgroundColor = Color.FromRgb(57, 208, 71);
                this.isButtonClicked = false;
                this.service.StopCreation();
            }
        }
    }
}
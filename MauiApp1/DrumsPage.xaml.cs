// <copyright file="DrumsPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using Plugin.Maui.Audio;

    /// <summary>
    ///     DrumsPage ContentPage.
    /// </summary>
    public partial class DrumsPage : ContentPage
    {
        private readonly IAudioManager audioManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DrumsPage"/> class.
        /// </summary>
        /// <param name="audioManager">Object of IAudioManager subtype.</param>
        public DrumsPage(IAudioManager audioManager)
        {
            this.InitializeComponent();
            this.audioManager = audioManager;
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Main");
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Main");
        }

        private async void DrumClicked(object sender, EventArgs e)
        {
            var player = this.audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Bass-Drum-1.wav"));
            player.Play();
        }
    }
}
// <copyright file="SaveConfirmationPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using MusicCreator.Services;

    /// <summary>
    ///     SaveConfirmationPage ContentPage.
    /// </summary>
    public partial class SaveConfirmationPage : ContentPage
    {
        private readonly Service service;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaveConfirmationPage"/> class.
        /// </summary>
        public SaveConfirmationPage()
        {
            this.InitializeComponent();
            this.service = Service.GetService();
        }

        private void OnYesClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.SaveTitleEntry.Text))
            {
                this.DisplayAlert("Empty title!", "Please enter a title!", "OK");
                return;
            }

            this.DisplayAlert("Your masterpiece has been saved!", "We are waiting for your creative mastermind to blossom once again", "OK");
            this.service.SaveCreation(this.SaveTitleEntry.Text);
            Shell.Current.GoToAsync("Main");
        }

        private void OnNoClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("Main");
        }
    }
}
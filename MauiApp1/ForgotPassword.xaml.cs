// <copyright file="ForgotPassword.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    /// <summary>
    ///     ForgotPassword ContentPage.
    /// </summary>
    public partial class ForgotPassword : ContentPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ForgotPassword"/> class.
        /// </summary>
        public ForgotPassword()
        {
            this.InitializeComponent();
        }

        private async void OnSentRecoverButton(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.UsernameEntry.Text) && this.UsernameEntry.Text.Contains("@"))
            {
                // UsernameEntry is not empty and contains "@", proceed with sending the code
                await this.DisplayAlert("Success", "Email sent", "OK");
            }
            else
            {
                // UsernameEntry is empty or does not contain "@", show an alert
                await this.DisplayAlert("Error", "Please enter a valid email address", "OK");
            }
        }
    }
}
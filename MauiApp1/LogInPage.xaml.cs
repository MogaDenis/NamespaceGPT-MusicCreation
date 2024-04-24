// <copyright file="LogInPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using MusicCreator.Services;

    /// <summary>
    ///     LoginPage ContentPage.
    /// </summary>
    public partial class LogInPage : ContentPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogInPage"/> class.
        /// </summary>
        public LogInPage()
        {
            this.InitializeComponent();
            Service.GetService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.UsernameEntry.Text) || string.IsNullOrWhiteSpace(this.PasswordEntry.Text))
            {
                await this.DisplayAlert("Error", "Username and password are required!", "OK");
                return;
            }

            await Shell.Current.GoToAsync("Main");
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ForgotPassword");
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("SignUp");
        }
    }
}

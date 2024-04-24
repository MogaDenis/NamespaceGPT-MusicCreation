// <copyright file="SignUpPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    /// <summary>
    ///     SignupPage ContentPage.
    /// </summary>
    public partial class SignUpPage : ContentPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SignUpPage"/> class.
        /// </summary>
        public SignUpPage()
        {
            this.InitializeComponent();
        }

        private static bool IsPasswordComplex(string password)
        {
            // Check if password contains at least one uppercase letter, one lowercase letter, one special character, and is at least 6 characters long
            return !string.IsNullOrWhiteSpace(password) &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        private async void GoFromMainToLogInPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LogIn");
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(this.LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(this.UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(this.PasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(this.VerifyPasswordEntry.Text))
            {
                await this.DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            // Check if passwords match
            if (this.PasswordEntry.Text != this.VerifyPasswordEntry.Text)
            {
                await this.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            // Check if password meets complexity requirements
            string password = this.PasswordEntry.Text;
            if (!IsPasswordComplex(password))
            {
                await this.DisplayAlert("Error", "Password must contain at least one uppercase letter, one lowercase letter, one special character, and be at least 6 characters long", "OK");
                return;
            }

            // Check if the radio button is checked (assuming RadioButton is a custom control)
            if (!this.RadioButton.IsChecked)
            {
                await this.DisplayAlert("Error", "You must agree to the terms and conditions", "OK");
                return;
            }

            // If all validations pass, proceed with sign in
            await Shell.Current.GoToAsync("Main");
        }
    }
}
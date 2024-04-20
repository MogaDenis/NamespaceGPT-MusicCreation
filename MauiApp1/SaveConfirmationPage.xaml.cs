using MusicCreator.Services;

namespace MusicCreator
{

    public partial class SaveConfirmationPage : ContentPage
    {
        private readonly Service _service;
        public SaveConfirmationPage()
        {
            InitializeComponent();
            _service = Service.GetService();
        }

        private void OnYesClicked(object sender, EventArgs e)
        {
            //see if there is title in the entry
            if (string.IsNullOrEmpty(SaveTitleEntry.Text))
            {
                DisplayAlert("Empty title!", "Please enter a title!", "OK");
                return;
            }
            DisplayAlert("Your masterpiece has been saved!", "We are waiting for your creative mastermind to blossom once again", "OK");
            _service.SaveCreation(SaveTitleEntry.Text);
            Shell.Current.GoToAsync("Main");
        }

        private void OnNoClicked(object sender, EventArgs e)
        {
            // Handle cancel action
            // Navigate back to the main page OR exit without saving 
            Shell.Current.GoToAsync("Main");
        }
    }
}
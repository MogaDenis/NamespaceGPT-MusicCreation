using Music.MusicDomain;
using MusicCreator.Services;

namespace MusicCreator
{
    public partial class MainPageApp : ContentPage
    {
        private readonly Service _service;
        private readonly List<Track> _tracks;
        private bool _isButtonClicked;

        public MainPageApp()
        {

            InitializeComponent();
            _service = Service.GetService();
            _tracks = _service.GetCreationTracks();
            List<string> items = (from t in _tracks
                                  select t.Title).ToList();

            tracksListView.ItemsSource = items;
            _isButtonClicked = false;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button { CommandParameter: string item } && tracksListView.ItemsSource is List<string> items)
            {
                items.Remove(item);
                var track = _tracks.Find(x => x.Title == item);
                if (track == null)
                {
                    return;
                }

                int trackId = track.Id;
                _service.RemoveTrack(trackId);
                tracksListView.ItemsSource = null;
                tracksListView.ItemsSource = items;
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
            _service.Category = category;
            await Shell.Current.GoToAsync("Search");
        }

        private async void GoFromMainToSavePage(object sender, EventArgs e)
        {
            if (_service.GetCreationTracks().Count == 0)
            {
                await DisplayAlert("Empty creation!", "Please select at least one track!", "OK");
                return;
            }
            await Shell.Current.GoToAsync("Save");
        }


        private void PlayCreation(object sender, EventArgs e)
        {
            if (!_isButtonClicked && _service.GetCreationTracks().Count() != 0)
            {
                playButton.BackgroundColor = Color.FromRgb(255, 0, 0);
                _isButtonClicked = true;
                _service.PlayCreation();
            }
            else
            {
                playButton.BackgroundColor = Color.FromRgb(57, 208, 71);
                _isButtonClicked = false;
                _service.StopCreation();
            }
        }
    }
}
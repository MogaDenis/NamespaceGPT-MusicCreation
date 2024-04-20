using Bumptech.Glide.Load;
using Music.MusicDomain;
using MusicCreator.Services;

namespace MusicCreator
{
    public partial class SearchPage : ContentPage
    {
        private readonly Service _service;
        private readonly int _categoryAsInt;

        public SearchPage()
        {
            InitializeComponent();

            _service = Service.GetService();

            string category = _service.Category;

            if (category == "drums")
            {
                _categoryAsInt = 1;
            }
            else if (category == "music")
            {
                _categoryAsInt = 2;
            }
            else if (category == "fx")
            {
                _categoryAsInt = 3;
            }
            else if (category == "mic")
            {
                _categoryAsInt = 4;
            }
            else
            {
                _categoryAsInt = 0;
            }

            var tracksData = _service.GetTracksByType(_categoryAsInt);
            TracksListView.ItemsSource = tracksData;


            //SearchBar.SearchButtonPressed += OnSearchButtonPressed;
        }

        public void OnTrackTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is not Track track)
            {
                return;
            }

            _service.AddTrack(track);
            _service.StopAll();

            Shell.Current.GoToAsync("Main");
        }

        public void OnPlayClicked(object sender, EventArgs e)
        {
            //TO DO
            //PLAY THE TRACK
            _service.StopAll();
            int id = (int)((Button)sender).CommandParameter;
            var track = _service.GetTrackById(id);
            if (track == null)
            {
                return;
            }

            track.Play();
        }

        public async void GoFromSearchToMainPage(object sender, EventArgs e)
        {
            _service.StopAll();
            await Shell.Current.GoToAsync("Main");
        }

        // Method to create dynamic buttons for sentences containing the search query
        /*private void CreateButtons(string searchQuery)
        {
            ButtonsLayout.Children.Clear(); // Clear existing buttons

            foreach (Track track in tracksData)
            {
                string title = track.getTitle();
                if (title.ToLower().Contains(searchQuery.ToLower()))
                {
                    var button = new Button { Text = title };
                    button.Clicked += Button_Clicked; // Add event handler for button click
                    ButtonsLayout.Children.Add(button);

                }
            }
        }
        */
        // Event handler for the search bar's search button pressed event
        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string searchQuery = SearchBar.Text;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {

                TracksListView.ItemsSource = _service.GetTracksByType(_categoryAsInt);
            }
            else
            {
                TracksListView.ItemsSource = _service.GetTracksByType(_categoryAsInt).
                    FindAll(x => x.Title.ToLower().Contains(searchQuery.ToLower()));

            }
        }
        /*
        // Event handler for dynamic button click
        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string track = button.Text;
            //list_of_tracks.Add(track);
            // You can do additional actions here if needed
            //TO DO
            //SEND BACK TO MAIN PAGE THE LIST OF TRACK
            Shell.Current.Navigation.PushAsync(new MainPageApp(track));
            }
        */

    }
}
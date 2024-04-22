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
                TracksListView.ItemsSource = _service.GetTracksByTypeAndFilterByTitle(_categoryAsInt, searchQuery);

                //TracksListView.ItemsSource = _service.GetTracksByType(_categoryAsInt).
                //    FindAll(track => track.Title.ToLower().Contains(searchQuery.ToLower()));
            }
        }
    }
}
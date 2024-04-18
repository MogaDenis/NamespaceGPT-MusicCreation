using MusicCreator.Repository;
using Music.MusicDomain;
using MusicCreator;
using MusicCreator.Services;
using System.Collections.ObjectModel;


namespace MusicCreator;

public partial class SearchPage : ContentPage
{

    Service service;
    List<Track> tracksData;
    int categoryInt;

    public SearchPage()
    {
        InitializeComponent();

        service = Service.GetService();

        string category = service.category;
        

        if (category == "drums")
        {
            categoryInt = 1;
        }
        else if(category == "music")
        {
            categoryInt = 2;
        }
        else if(category == "fx")
        {
            categoryInt = 3;
        }
        else if(category == "mic")
        {
            categoryInt = 4;
        }
        else
        {
            categoryInt = 0;
        }

        tracksData = service.GetTracksByType(categoryInt);
        TracksListView.ItemsSource = tracksData;
        

        //SearchBar.SearchButtonPressed += OnSearchButtonPressed;
    }

    

    public void OnTrackTapped(object sender, ItemTappedEventArgs e)
    {
        Track track = e.Item as Track;
        service.AddTrack(track);
        service.StopAll();
       
        Shell.Current.GoToAsync("Main");
    }

    public void OnPlayClicked(object sender, EventArgs e)
    {

        //TO DO
        //PLAY THE TRACK
        service.StopAll();
        int id = (int)((Button)sender).CommandParameter;
        Track track = service.GetTrackById(id);
        track.Play();

    }

    public async void GoFromSearchToMainPage(object sender, EventArgs e)
    {
        service.StopAll();
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
            
            TracksListView.ItemsSource = service.GetTracksByType(categoryInt);
        }
        else
        {
            TracksListView.ItemsSource = service.GetTracksByType(categoryInt).
                FindAll(x => x.getTitle().ToLower().Contains(searchQuery.ToLower()));

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
using Plugin.Maui.Audio;

namespace MusicCreator;

public partial class DrumsPage : ContentPage
{
    private readonly IAudioManager audioManager;

    public DrumsPage(IAudioManager audioManager)
	{
		InitializeComponent();
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
    private async void Drum1Clicked(object sender, EventArgs e)
    {
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Bass-Drum-1.wav"));
        player.Play();
        //await Shell.Current.GoToAsync("Main");
    }
}
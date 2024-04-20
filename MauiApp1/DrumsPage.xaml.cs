using Plugin.Maui.Audio;

namespace MusicCreator
{
    public partial class DrumsPage : ContentPage
    {
        private readonly IAudioManager _audioManager;

        public DrumsPage(IAudioManager audioManager)
        {
            InitializeComponent();
            this._audioManager = audioManager;
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
            var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Bass-Drum-1.wav"));
            player.Play();
            //await Shell.Current.GoToAsync("Main");
        }
    }
}
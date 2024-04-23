using MusicCreator;
using MusicCreator.Repository;
using MusicCreator.Services;

namespace MusicCreator
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Search", typeof(SearchPage));
            Routing.RegisterRoute("Main", typeof(MainPageApp));
            Routing.RegisterRoute("LogIn", typeof(LogInPage));
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            Routing.RegisterRoute("Save", typeof(SaveConfirmationPage));
            Routing.RegisterRoute("ForgotPassword", typeof(ForgotPassword));

            var sqlConnectionFactory = new SqlConnectionFactory();

            var trackRepository = new TrackRepository(sqlConnectionFactory);
            var creationRepository = new CreationRepository();
            var songRepository = new SongRepository(sqlConnectionFactory);

            _ = new Service(trackRepository, creationRepository, songRepository);
        }
    }
}

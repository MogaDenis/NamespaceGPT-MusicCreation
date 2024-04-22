using MusicCreator;

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
        }
    }
}

// <copyright file="AppShell.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using MusicCreator.Repository;
    using MusicCreator.Services;

    /// <summary>
    ///     AppShell class which initializes all routes and the service.
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            this.InitializeComponent();

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

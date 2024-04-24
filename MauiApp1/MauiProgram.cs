// <copyright file="MauiProgram.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MusicCreator
{
    using Microsoft.Extensions.Logging;
    using Plugin.Maui.Audio;

    /// <summary>
    ///     Entry point of the MauiProgra.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        ///     Factory method which creates a MauiApp instance.
        /// </summary>
        /// <returns>A new MauiApp.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<DrumsPage>();
#endif

            return builder.Build();
        }
    }
}

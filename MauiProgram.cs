using MAUI_Tutorial1_TodoList.ViewModel;
using MAUI_Tutorial1_TodoList.Views;
using MAUI_Tutorial1_TodoList.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using DotNetEnv;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage; // For SecureStorage

namespace MAUI_Tutorial1_TodoList
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("MAUI_Tutorial1_TodoList.env");
            if (stream is null)
                System.Diagnostics.Debug.WriteLine("⚠️ .env not found as embedded resource");
            else
                Env.Load(stream);

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddHttpClient<IPetBackendService, PetBackendService>(client =>
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7291/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true,
                UseCookies = false
            });

            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

            builder.Services.AddHttpClient<IPetfinderService, PetfinderService>();
            builder.Services.AddHttpClient<IPetplaceService, PetPlaceService>();
            builder.Services.AddHttpClient<IAnimalDetailService, AnimalDetailService>();
            builder.Services.AddHttpClient<UserService>(client =>
            {
                // CRITICAL: Adjust this URL according to your environment.
                client.BaseAddress = new Uri("https://10.0.2.2:7291/"); // Android emulator

                var token = SecureStorage.GetAsync("auth_token").Result;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true // Dev only!
            });

            // Pages & ViewModels
            builder.Services.AddSingleton<AllPetsPage>();
            builder.Services.AddSingleton<CombinedPetsViewModel>();

            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<DetailViewModel>();

            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<FavoritesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

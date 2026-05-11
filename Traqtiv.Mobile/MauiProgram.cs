using Microsoft.Extensions.Logging;
using Traqtiv.Mobile.Services;
using Traqtiv.Mobile.ViewModels;
using Traqtiv.Mobile.Views;

namespace Traqtiv.Mobile;

public static class MauiProgram
{
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

        // HttpClient
        builder.Services.AddHttpClient("TraqtivApi", client =>
        {
            client.BaseAddress = new Uri(AppConstants.ApiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Services - Singleton 
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IWorkoutService, WorkoutService>();
        builder.Services.AddSingleton<IDailyActivityService, DailyActivityService>();
        builder.Services.AddSingleton<IBodyMetricsService, BodyMetricsService>();
        builder.Services.AddSingleton<IRecommendationService, RecommendationService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels - Transient 
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<WorkoutsViewModel>();
        builder.Services.AddTransient<AddWorkoutViewModel>();
        builder.Services.AddTransient<DailyActivityViewModel>();
        builder.Services.AddTransient<BodyMetricsViewModel>();
        builder.Services.AddTransient<RecommendationsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Pages - Transient
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<WorkoutsPage>();
        builder.Services.AddTransient<AddWorkoutPage>();
        builder.Services.AddTransient<DailyActivityPage>();
        builder.Services.AddTransient<BodyMetricsPage>();
        builder.Services.AddTransient<RecommendationsPage>();
        builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
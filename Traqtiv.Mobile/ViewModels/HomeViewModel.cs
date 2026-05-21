using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;


// This ViewModel manages the data and interactions for the Home view of the application, including displaying recent workouts, today's activity, and active alerts.
public partial class HomeViewModel : BaseViewModel
{
    private readonly IWorkoutService _workoutService;
    private readonly IDailyActivityService _activityService;
    private readonly IRecommendationService _recommendationService;
    private readonly INavigationService _navigationService;
    private readonly IWeatherService _weatherService;


    [ObservableProperty]
    private List<WorkoutDto> _recentWorkouts = new();

    [ObservableProperty]
    private DailyActivityDto? _todayActivity;

    [ObservableProperty]
    private List<AlertDto> _activeAlerts = new();

    [ObservableProperty]
    private WeatherResponseDto? _currentWeather;

    [ObservableProperty]
    private string _weatherIcon = "🌤️";

    [ObservableProperty]
    private string _cityName = string.Empty;

    [ObservableProperty]
    private string _outdoorRecommendation = string.Empty;



    // Initializes a new instance of the HomeViewModel class, injecting the necessary services for workouts, daily activity, recommendations, and navigation.
    public HomeViewModel(IWorkoutService workoutService,IDailyActivityService activityService,IRecommendationService recommendationService,INavigationService navigationService , IWeatherService weatherService)
    {
        _workoutService = workoutService;
        _activityService = activityService;
        _recommendationService = recommendationService;
        _navigationService = navigationService;
        _weatherService = weatherService;
        Title = "Home";
        ActiveTab = "home";
    }

    // This command loads the data for the Home view, including recent workouts, today's activity, and active alerts.
    // It checks for connectivity before attempting to load data and handles the busy state to prevent multiple simultaneous loads.
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;

        if (!await ConnectivityHelper.CheckAndAlertAsync()) 
            return;

        // The method uses Task.WhenAll to load workouts, activities, and alerts concurrently, improving performance.
        // It then processes the results to update the RecentWorkouts, TodayActivity, and ActiveAlerts properties.
        try
        {
            IsBusy = true;

            var workoutsTask = _workoutService.GetWorkoutsAsync();
            var activitiesTask = _activityService.GetActivitiesAsync();
            var alertsTask = _recommendationService.GetAlertsAsync();


            await Task.WhenAll(workoutsTask, activitiesTask, alertsTask);

            RecentWorkouts = workoutsTask.Result.OrderByDescending(w => w.Date).Take(3).ToList(); // Display the 3 most recent workouts

            TodayActivity = activitiesTask.Result.FirstOrDefault(a => a.Date.Date == DateTime.Today); // Get today's activity

            ActiveAlerts = alertsTask.Result.Where(a => !a.IsRead).ToList(); // Display only active (unread) alerts
        }
        finally
        {
            IsBusy = false;
        }
        await LoadWeatherAsync(); // Load weather data after loading other data to ensure the UI is responsive.
    }

    // This method retrieves the current weather data based on the user's location and updates the CurrentWeather and WeatherIcon properties accordingly.
    private async Task LoadWeatherAsync()
    {
        try
        {
            var location = await LocationHelper.GetCurrentLocationAsync();

            if (location != null)
            {
                var cityTask = LocationHelper.GetCityNameAsync(location.Value.latitude, location.Value.longitude);
                var weatherTask = _weatherService.GetCurrentWeatherAsync(location.Value.latitude,location.Value.longitude);
                await Task.WhenAll(cityTask, weatherTask);

                CityName = cityTask.Result;
                var weather = weatherTask.Result;
                if (weather != null)
                {
                    CurrentWeather = weather;
                    WeatherIcon = WeatherHelper.GetWeatherIcon(weather.Description); // Convert weather description to an emoji icon for display in the UI.

                    // Get outdoor workout recommendation based on temperature and air quality index, providing users with actionable advice on whether it's suitable for outdoor exercise.
                    OutdoorRecommendation = WeatherHelper.GetOutdoorRecommendation(weather.Temperature, weather.AirQualityIndex); 
                }
            }
        }
        catch (Exception)
        {
            // weather data is optional, so we can ignore exceptions here

        }

    }

    // This command navigates the user to the Recommendations view when executed.
    [RelayCommand]
    private async Task NavigateToRecommendationsAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.Recommendations);
    }
}

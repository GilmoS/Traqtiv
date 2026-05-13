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

    [ObservableProperty]
    private List<WorkoutDto> _recentWorkouts = new();

    [ObservableProperty]
    private DailyActivityDto? _todayActivity;

    [ObservableProperty]
    private List<AlertDto> _activeAlerts = new();


    // Initializes a new instance of the HomeViewModel class, injecting the necessary services for workouts, daily activity, recommendations, and navigation.
    public HomeViewModel(IWorkoutService workoutService,IDailyActivityService activityService,IRecommendationService recommendationService,INavigationService navigationService)
    {
        _workoutService = workoutService;
        _activityService = activityService;
        _recommendationService = recommendationService;
        _navigationService = navigationService;
        Title = "Home";
    }

    // This command loads the data for the Home view, including recent workouts, today's activity, and active alerts.
    // It checks for connectivity before attempting to load data and handles the busy state to prevent multiple simultaneous loads.
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;
        if (!await ConnectivityHelper.CheckAndAlertAsync()) return;
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
    }

    // This command navigates the user to the Workouts view when executed.
    [RelayCommand]
    private async Task NavigateToWorkoutsAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.Workouts);
    }


    // This command navigates the user to the Recommendations view when executed.
    [RelayCommand]
    private async Task NavigateToRecommendationsAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.Recommendations);
    }
}

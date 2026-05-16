using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;
//This view model is responsible for managing the data and logic related to the daily activity page,
//including fetching the user's activity data, calculating progress towards goals, and handling user interactions such as saving new activity entries or navigating to detailed views of past activities.
public partial class DailyActivityViewModel : BaseViewModel
{
    private readonly IDailyActivityService _activityService;
    private readonly HealthService _healthService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private DailyActivityDto? _todayActivity;

    [ObservableProperty]
    private ActivitySummaryDto? _weeklySummary;

    [ObservableProperty]
    private int _steps;

    [ObservableProperty]
    private int _caloriesBurned;

    [ObservableProperty]
    private int _activeMinutes;

    [ObservableProperty]
    private double _distanceKm;

    // Constructor with dependency injection for services
    public DailyActivityViewModel(IDailyActivityService activityService,HealthService healthService,INavigationService navigationService)
    {
        _activityService = activityService;
        _healthService = healthService;
        _navigationService = navigationService;
        Title = "Daily Activity";
        ActiveTab = "activity";
    }

    // Property to display today's date in a user-friendly format
    public string TodayDate => DateTime.Now.ToString("MMMM dd, yyyy");

    // Method to load today's activity and weekly summary data
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;

        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;

        // Set default values to prevent null reference issues in the UI
        try
        {
            IsBusy = true;

            var activitiesTask = _activityService.GetActivitiesAsync();
            var summaryTask = _activityService.GetActivitySummaryAsync(
                DateTime.Today.AddDays(-7), DateTime.Today);

            await Task.WhenAll(activitiesTask, summaryTask);

            TodayActivity = activitiesTask.Result
                .FirstOrDefault(a => a.Date.Date == DateTime.Today);

            WeeklySummary = summaryTask.Result;

            //If data from device is available
            var healthData = await _healthService.GetTodayActivityAsync();
            if (healthData != null && TodayActivity == null)
            {
                Steps = healthData.Steps;
                CaloriesBurned = healthData.CaloriesBurned;
                ActiveMinutes = healthData.ActiveMinutes;
                DistanceKm = healthData.DistanceKm;
            }
            else if (TodayActivity != null)
            {
                Steps = TodayActivity.Steps;
                CaloriesBurned = TodayActivity.CaloriesBurned;
                ActiveMinutes = TodayActivity.ActiveMinutes;
                DistanceKm = TodayActivity.DistanceKm;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Method to save the current activity data entered by the user
    [RelayCommand]
    private async Task SaveActivityAsync()
    {
        if (IsBusy)
            return;

        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;

        // Validate input data
        try
        {
            IsBusy = true;

            var request = new AddDailyActivityDto
            {
                Steps = Steps,
                CaloriesBurned = CaloriesBurned,
                ActiveMinutes = ActiveMinutes,
                DistanceKm = DistanceKm,
                Date = DateTime.Today
            };

            var success = await _activityService.AddDailyActivityAsync(request); //This method sends the new activity data to the backend API to be saved in the database. It returns a boolean indicating whether the save operation was successful or not.

            if (success)
            {
                await AlertHelper.ShowSuccessAsync("Activity saved successfully!");
                await LoadDataAsync();
            }
            else
                await AlertHelper.ShowErrorAsync("Failed to save activity.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
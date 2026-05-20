using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;


//This ViewModel manages the user's profile information and handles the logout functionality.
public partial class ProfileViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IUserService _userService;
    private readonly IWorkoutService _workoutService;
    private readonly IDailyActivityService _activityService;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private DateTime _dateOfBirth;

    [ObservableProperty]
    private int _totalWorkouts;

    [ObservableProperty]
    private int _totalSteps;

    [ObservableProperty]
    private int _totalCalories;

    public string FullName => $"{FirstName} {LastName}";// A read-only property that combines the first name and last name to display the user's full name.


    //Constructor that initializes the services and sets the title of the ViewModel to "Profile".
    public ProfileViewModel(IAuthService authService,INavigationService navigationService,IUserService userService,IWorkoutService workoutService,IDailyActivityService activityService)
    {
        _authService = authService;
        _navigationService = navigationService;
        _userService = userService;
        _workoutService = workoutService;
        _activityService = activityService;
        Title = "Profile";
    }

    // This method loads the user's profile information from the authentication service.
    // It checks for connectivity before making the API call and updates the properties with the retrieved data.
    [RelayCommand]
    private async Task LogoutAsync()
    {
        if (IsBusy)
            return;

        var confirmed = await AlertHelper.ShowConfirmAsync("Logout","Are you sure you want to logout?"); // Show a confirmation dialog to the user before logging out and check if they confirmed the action.

        if (!confirmed) // If the user did not confirm the logout action, simply return without doing anything.
            return;

        try
        {
            IsBusy = true;
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync(AppConstants.Routes.Login);
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This method is a placeholder for loading the user's profile data. It sets the active tab to "profile" and simulates an asynchronous operation.
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;
        // Check for connectivity before attempting to load data. If there is no connectivity, show an alert and return without trying to load the profile data.
        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;
        try
        {
            IsBusy = true;
            ActiveTab = "profile";

            var profiletask = _userService.GetProfileAsync();
            var workoutsTask = _workoutService.GetWorkoutsAsync();
            var summaryTask = _activityService.GetActivitySummaryAsync(DateTime.Today.AddDays(-30), DateTime.Today);

            await Task.WhenAll(profiletask, workoutsTask, summaryTask);

            var profile = profiletask.Result;

            if (profile != null)
            {
                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Email = profile.Email;
                DateOfBirth = profile.DateOfBirth.DateTime;
                OnPropertyChanged(nameof(FullName));
            }

            TotalWorkouts = workoutsTask.Result.Count;
            var summary = summaryTask.Result;

            if (summary != null)
            {
                TotalSteps = summary.TotalSteps;
                TotalCalories = summary.TotalCaloriesBurned;
            }

            
         }
        finally
        {
            IsBusy = false;
        }

    }

    

    // This method is a placeholder for saving the user's profile data. It simulates an asynchronous operation and shows a success message when the profile is updated.
    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (IsBusy) return;

        var confirmed = await AlertHelper.ShowConfirmAsync("Logout", "Are you sure you want to logout?");
        if (!confirmed) return;

        try
        {
            IsBusy = true;
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync(AppConstants.Routes.Login);
        }
        finally
        {
            IsBusy = false;
        }
    }












}
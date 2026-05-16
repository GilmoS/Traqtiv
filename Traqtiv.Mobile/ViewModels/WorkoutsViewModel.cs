using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;


// This ViewModel manages the list of workouts, including loading workouts from the service, adding new workouts, deleting existing ones, and navigating to workout details.
// It uses CommunityToolkit.Mvvm for property change notifications and command handling, making it easier to manage the state and actions related to workouts in the application.
public partial class WorkoutsViewModel : BaseViewModel
{
    private readonly IWorkoutService _workoutService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private List<WorkoutDto> _workouts = new();

    [ObservableProperty]
    private WorkoutDto? _selectedWorkout;

    // Initializes a new instance of the WorkoutsViewModel class, injecting the workout and navigation services.
    public WorkoutsViewModel( IWorkoutService workoutService,INavigationService navigationService)
    {
        _workoutService = workoutService;
        _navigationService = navigationService;
        Title = "Workouts";
        ActiveTab = "workouts";
    }

    // This command is executed when the view appears, triggering the loading of workouts from the service.
    [RelayCommand]
    private async Task LoadWorkoutsAsync()
    {
        if (IsBusy) return;
        if (!await ConnectivityHelper.CheckAndAlertAsync()) return;

        // Load workouts from the service and update the Workouts property, while managing the IsBusy state to prevent multiple simultaneous loads.
        try
        {
            IsBusy = true;
            Workouts = await _workoutService.GetWorkoutsAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This command is executed when the user wants to add a new workout, navigating to the AddWorkout view.
    [RelayCommand]
    private async Task AddWorkoutAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.AddWorkout);
    }

    // This command is executed when the user wants to delete a workout, showing a confirmation dialog and then calling the service to delete the workout if confirmed.
    [RelayCommand]
    private async Task DeleteWorkoutAsync(WorkoutDto workout)
    {
        if (IsBusy) return;

        var confirmed = await AlertHelper.ShowConfirmAsync(
            "Delete Workout",
            "Are you sure you want to delete this workout?");

        if (!confirmed) return;

        // Call the service to delete the workout and reload the workouts list if successful, while managing the IsBusy state to prevent multiple simultaneous deletions.
        try
        {
            IsBusy = true;

            var success = await _workoutService.DeleteWorkoutAsync(workout.Id);

            if (success)
                await LoadWorkoutsAsync();
            else
                await AlertHelper.ShowErrorAsync("Failed to delete workout.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This command is executed when the user selects a workout, setting the SelectedWorkout property and navigating to the AddWorkout view with the selected workout's ID for editing.
    [RelayCommand]
    private async Task SelectWorkoutAsync(WorkoutDto workout)
    {
        SelectedWorkout = workout;
        await _navigationService.NavigateToAsync( $"{AppConstants.Routes.AddWorkout}?id={workout.Id}"); // Navigate to the AddWorkout view with the selected workout's ID for editing.
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;

// This ViewModel manages the state and logic for adding or editing a workout, including handling user input, interacting with the workout service, and navigating back to the workouts list.
[QueryProperty(nameof(WorkoutId), "id")]
public partial class AddWorkoutViewModel : BaseViewModel
{
    private readonly IWorkoutService _workoutService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _workoutId = string.Empty;

    [ObservableProperty]
    private WorkoutType _selectedType = WorkoutType.Strength;

    [ObservableProperty]
    private int _durationMinutes;

    [ObservableProperty]
    private WorkoutStatus _selectedStatus = WorkoutStatus.Completed;

    [ObservableProperty]
    private int _caloriesBurned;

    [ObservableProperty]
    private DateTime _date = DateTime.Today;

    [ObservableProperty]
    private string _notes = string.Empty;

    [ObservableProperty]
    private bool _isEditing;

    public List<WorkoutType> WorkoutTypes =>Enum.GetValues<WorkoutType>().ToList();

    public List<WorkoutStatus> WorkoutStatuses =>Enum.GetValues<WorkoutStatus>().ToList();


    // Initializes a new instance of the AddWorkoutViewModel class, injecting the workout and navigation services, and setting the initial title of the view to "Add Workout".
    public AddWorkoutViewModel(IWorkoutService workoutService,INavigationService navigationService)
    {
        _workoutService = workoutService;
        _navigationService = navigationService;
        Title = "Add Workout";
    }

    // This method is called automatically when the WorkoutId property changes, indicating that the user is editing an existing workout.
    partial void OnWorkoutIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            IsEditing = true;
            Title = "Edit Workout";
            LoadWorkoutAsync(Guid.Parse(value)).FireAndForget(); // Load the workout details for editing without awaiting, allowing the UI to remain responsive while the data is being loaded.
        }
    }
    
    // This method loads the workout details for editing, given the workout ID.
    private async Task LoadWorkoutAsync(Guid id)
    {
        // Load the workout details from the service and populate the properties for editing, while managing the IsBusy state to prevent multiple simultaneous loads.
        try
        {
            IsBusy = true;
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            if (workout != null)
            {
                SelectedType = workout.Type;
                DurationMinutes = workout.DurationMinutes;
                SelectedStatus = workout.Status;
                CaloriesBurned = workout.CaloriesBurned;
                Date = workout.Date;
                Notes = workout.Notes;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This command is executed when the user attempts to save the workout, performing validation,
    // checking for connectivity, and interacting with the workout service to either add a new workout or update an existing one.
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy) return;

        // Validate user input before attempting to save the workout, ensuring that all required fields are filled in and that the duration is a positive number.
        if (DurationMinutes <= 0)
        {
            await AlertHelper.ShowErrorAsync("Please enter a valid duration.");
            return;
        }

        if (!await ConnectivityHelper.CheckAndAlertAsync()) return;

        // Call the service to save the workout and navigate back to the workouts list if successful, while managing the IsBusy state to prevent multiple simultaneous saves.
        try
        {
            IsBusy = true;
            bool success;

            if (IsEditing)// If editing an existing workout, create an UpdateWorkoutDto and call the update method on the service.
            {
                var request = new UpdateWorkoutDto
                {
                    Type = SelectedType,
                    DurationMinutes = DurationMinutes,
                    Status = SelectedStatus,
                    CaloriesBurned = CaloriesBurned,
                    Date = Date,
                    Notes = Notes
                };
                success = await _workoutService.UpdateWorkoutAsync(Guid.Parse(WorkoutId), request);
            }
            else // If adding a new workout, create an AddWorkoutDto and call the add method on the service.
            {
                var request = new AddWorkoutDto
                {
                    Type = SelectedType,
                    DurationMinutes = DurationMinutes,
                    Status = SelectedStatus,
                    CaloriesBurned = CaloriesBurned,
                    Date = Date,
                    Notes = Notes
                };
                success = await _workoutService.AddWorkoutAsync(request);
            }

            if (success)
                await _navigationService.GoBackAsync();
            else
                await AlertHelper.ShowErrorAsync("Failed to save workout.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This command is executed when the user decides to cancel adding or editing a workout, navigating back to the workouts list without saving any changes.
    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync();
    }
}
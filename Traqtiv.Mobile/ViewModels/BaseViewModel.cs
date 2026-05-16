using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;

namespace Traqtiv.Mobile.ViewModels;

// The BaseViewModel class serves as a common base for all view models in the application, providing shared properties and commands for navigation and state management.
// It inherits from ObservableObject, which allows it to notify the UI of property changes, enabling data binding and reactive UI updates.
// It includes properties for tracking the busy state of the view model, the title of the page, and the active tab in the bottom navigation bar.
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _activeTab = string.Empty;

    public bool IsNotBusy => !IsBusy;

    // The following commands are used for navigation to different pages in the application.
    // They utilize the Shell navigation system to navigate to the specified routes defined in AppConstants.
    [RelayCommand]
    private async Task NavigateHomeAsync()
    {
        await Shell.Current.GoToAsync(AppConstants.Routes.Home);
    }

    // Each of these commands corresponds to a different page in the app, allowing users to easily navigate between the main sections of the application.
    [RelayCommand]
    private async Task NavigateToWorkoutsAsync()
    {
        await Shell.Current.GoToAsync(AppConstants.Routes.Workouts);
    }

    [RelayCommand]
    private async Task NavigateToDailyActivityAsync()
    {
        await Shell.Current.GoToAsync(AppConstants.Routes.DailyActivity);
    }

    // This command navigates to the Body Metrics page, where users can view and manage their body measurements and related data.
    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await Shell.Current.GoToAsync(AppConstants.Routes.Profile);
    }
}
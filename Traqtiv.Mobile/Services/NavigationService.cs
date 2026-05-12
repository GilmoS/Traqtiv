using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Services;

// This service is responsible for handling navigation operations within the application, such as navigating to different pages and going back to the previous page.
// It uses the Shell.Current.GoToAsync method to perform navigation actions.
public class NavigationService : INavigationService
{
    // Initializes the service. In this case, it simply returns a completed task, but it can be extended to perform any necessary setup.
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    // Navigates to a specified route within the application.
    public async Task NavigateToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    // Navigates back to the previous page in the navigation stack.
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
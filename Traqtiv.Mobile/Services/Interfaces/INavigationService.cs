namespace Traqtiv.Mobile.Services.Interfaces;

// Interface for navigation services, providing methods to navigate to a specific route and to go back in the navigation stack.
public interface INavigationService : IService
{
    // Navigates to a specified route asynchronously.
    Task NavigateToAsync(string route);

    // Navigates back in the navigation stack asynchronously.
    Task GoBackAsync();
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;

// The LoginViewModel is responsible for handling the login logic of the application. It interacts with the authentication service to perform login operations and the navigation service to navigate between views.
// It also includes properties for email and password input, and commands for login and navigation to the registration view.
public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    // Constructor that initializes the authentication and navigation services, and sets the title of the view.
    public LoginViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Login";
    }

    // Command that handles the login process.
    // It checks for input validation, connectivity, and then attempts to log in using the authentication service.
    // If successful, it navigates to the home view; otherwise, it shows an error message.
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await AlertHelper.ShowErrorAsync("Please enter your email and password.");
            return;
        }

        if (!await ConnectivityHelper.CheckAndAlertAsync()) return;

        // Attempt to log in and handle the result accordingly, ensuring that the busy state is properly managed throughout the process.
        try
        {
            IsBusy = true; // Set the busy state to true to indicate that a login operation is in progress.

            var success = await _authService.LoginAsync(Email, Password);

            if (success)
                await _navigationService.NavigateToAsync(AppConstants.Routes.Home);
            else
                await AlertHelper.ShowErrorAsync("Invalid email or password.");
        }
        finally
        {
            IsBusy = false; // Ensure that the busy state is reset to false after the login attempt, regardless of the outcome.
        }
    }

    // Command that navigates to the registration view when the user wants to create a new account.
    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.Register);
    }
}
}
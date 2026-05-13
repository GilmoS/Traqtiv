using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;


// This ViewModel handles the user registration process, including input validation, interaction with the authentication service, and navigation to other views.
// It uses CommunityToolkit.Mvvm for property change notifications and command handling, making it easier to manage the state and actions related to user registration in the application.
public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] 
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private DateTime _dateOfBirth = DateTime.Today.AddYears(-18);


    /// Initializes a new instance of the RegisterViewModel class, injecting the authentication and navigation services.
    /// It also sets the title of the view to "Register".
    public RegisterViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Register";
    }

    // This command is executed when the user attempts to register.
    // It performs input validation, checks for connectivity, and interacts with the authentication service to register the user.
    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return; // Prevent multiple simultaneous registrations

        if (string.IsNullOrWhiteSpace(FirstName) ||string.IsNullOrWhiteSpace(LastName) ||string.IsNullOrWhiteSpace(Email) ||string.IsNullOrWhiteSpace(Password))
        {
            await AlertHelper.ShowErrorAsync("Please fill in all fields.");
            return;
        }

        if (Password != ConfirmPassword)
        {
            await AlertHelper.ShowErrorAsync("Passwords do not match.");
            return;
        }

        if (!await ConnectivityHelper.CheckAndAlertAsync()) return;

        // Attempt to register the user using the authentication service.
        // If successful, navigate to the home view; otherwise, show an error message.
        try
        {
            IsBusy = true;

            var success = await _authService.RegisterAsync(FirstName, LastName, Email, Password, DateOfBirth);

            if (success)
                await _navigationService.NavigateToAsync(AppConstants.Routes.Home);
            else
                await AlertHelper.ShowErrorAsync("Registration failed. Please try again.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This command is executed when the user wants to navigate to the login view.
    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await _navigationService.NavigateToAsync(AppConstants.Routes.Login);
    }
}
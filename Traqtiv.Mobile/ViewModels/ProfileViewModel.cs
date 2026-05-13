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

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private DateTime _dateOfBirth;


    // Constructor that initializes the authentication and navigation services, and sets the title of the ViewModel.
    public ProfileViewModel(IAuthService authService,INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
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
}
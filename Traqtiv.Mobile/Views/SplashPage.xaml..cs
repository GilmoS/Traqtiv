uusing Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.Views;

//This page is displayed when the app is launched.
//It checks if the user is logged in and navigates to the appropriate page (Home or Login) after a short delay.
public partial class SplashPage : ContentPage
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    // Constructor that initializes the services and components.
    public SplashPage(IAuthService authService, INavigationService navigationService)
    {
        InitializeComponent();
        _authService = authService;
        _navigationService = navigationService;
    }

    // This method is called when the page appears. It initializes the app and checks the login status.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeAppAsync();
    }
    // This method simulates a loading process and checks if the user is logged in.
    // It then navigates to the appropriate page based on the login status.
    private async Task InitializeAppAsync()
    {
        //wait for 1.5 seconds to simulate loading time (e.g., fetching data, initializing services, etc.)
        await Task.Delay(1500);

        // Check if the user is logged in by calling the IsLoggedInAsync method from the SecureStorageHelper.
        var isLoggedIn = await SecureStorageHelper.IsLoggedInAsync();

        if (isLoggedIn)
            await _navigationService.NavigateToAsync(AppConstants.Routes.Home);
        else
            await _navigationService.NavigateToAsync(AppConstants.Routes.Login);
    }
}

using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

// This class represents the login page of the application.
// It is responsible for displaying the login UI and binding to the LoginViewModel for handling user interactions and data.
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
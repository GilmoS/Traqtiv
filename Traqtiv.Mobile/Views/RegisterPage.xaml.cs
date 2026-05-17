using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

// This class represents the registration page of the application.
public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
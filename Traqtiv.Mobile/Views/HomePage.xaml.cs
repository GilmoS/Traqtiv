using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

// This class represents the home page of the application.
public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
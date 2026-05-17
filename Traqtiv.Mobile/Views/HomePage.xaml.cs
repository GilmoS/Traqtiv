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
    // This method is called when the page appears.
    // It checks if the BindingContext is of type HomeViewModel and if so, it executes the LoadDataCommand to load the necessary data for the page.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is HomeViewModel vm)
            await vm.LoadDataCommand.ExecuteAsync(null);
    }

}
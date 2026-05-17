using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class BodyMetricsPage : ContentPage
{
    public BodyMetricsPage(BodyMetricsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    // This method is called when the page appears.
    // It checks if the BindingContext is of type BodyMetricsViewModel and if so, it executes the LoadDataCommand to load the necessary data for the page.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BodyMetricsViewModel vm)
            await vm.LoadDataCommand.ExecuteAsync(null);
    }
}
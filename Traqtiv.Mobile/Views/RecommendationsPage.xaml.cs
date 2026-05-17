using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class RecommendationsPage : ContentPage
{
    public RecommendationsPage(RecommendationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // This method is called when the page appears.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RecommendationsViewModel vm)
            await vm.LoadDataCommand.ExecuteAsync(null);
    }
}
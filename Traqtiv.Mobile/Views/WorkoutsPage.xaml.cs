using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class WorkoutsPage : ContentPage
{
    public WorkoutsPage(WorkoutsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    // This method is called when the page appears.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is WorkoutsViewModel vm)
            await vm.LoadWorkoutsCommand.ExecuteAsync(null);
    }
}
using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

//This page is used to show the daily activity of the user, including their progress towards their goals and any achievements they have earned.
//It also allows the user to view their activity history and see how they have been progressing over time.
public partial class DailyActivityPage : ContentPage
{
    public DailyActivityPage(DailyActivityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // This method is called when the page appears.
    // It checks if the BindingContext is of type DailyActivityViewModel and if so, it executes the LoadDataCommand to load the necessary data for the page.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DailyActivityViewModel vm)
            await vm.LoadDataCommand.ExecuteAsync(null);
    }

}
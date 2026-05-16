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
}
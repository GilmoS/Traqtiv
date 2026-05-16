using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class WorkoutsPage : ContentPage
{
    public WorkoutsPage(WorkoutsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
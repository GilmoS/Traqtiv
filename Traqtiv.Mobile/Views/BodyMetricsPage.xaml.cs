using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class BodyMetricsPage : ContentPage
{
    public BodyMetricsPage(BodyMetricsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;

public partial class RecommendationsPage : ContentPage
{
    public RecommendationsPage(RecommendationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
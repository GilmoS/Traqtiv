using Traqtiv.Mobile.ViewModels;

namespace Traqtiv.Mobile.Views;
// The AddWorkoutPage is a view that allows users to add a new workout.
// It is bound to the AddWorkoutViewModel, which handles the logic for adding workouts, including input validation and interaction with the workout service.
public partial class AddWorkoutPage : ContentPage
{
    public AddWorkoutPage(AddWorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
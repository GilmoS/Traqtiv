namespace Traqtiv.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation. This allows the app to navigate to these pages using their route names.
        Routing.RegisterRoute(AppConstants.Routes.AddWorkout,
            typeof(Views.AddWorkoutPage));
    }
}
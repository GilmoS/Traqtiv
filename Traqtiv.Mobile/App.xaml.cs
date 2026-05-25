
namespace Traqtiv.Mobile;
// This is the main entry point of the application.
// It initializes the app and creates the main window using the AppShell as its content.
public partial class App : Application
{
    private readonly AppShell _shell;
    public App(AppShell shell)
    {
        InitializeComponent();
        _shell = shell;
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_shell);
    }
}
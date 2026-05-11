using CommunityToolkit.Mvvm.ComponentModel;

namespace Traqtiv.Mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    // base view model that other view models can inherit from, providing common properties and functionality
    // using CommunityToolkit.Mvvm to generate observable properties and notify property changed events
    // properties for tracking busy state and title of the view

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    public bool IsNotBusy => !IsBusy;
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;

//This ViewModel manages the recommendations and alerts for the user.
//It interacts with the IRecommendationService to fetch the latest recommendations and alerts, and allows users to mark alerts as read.
//The ViewModel includes error handling and user feedback mechanisms to ensure a smooth user experience.

public partial class RecommendationsViewModel : BaseViewModel
{
    private readonly IRecommendationService _recommendationService;

    [ObservableProperty]
    private List<RecommendationDto> _recommendations = new();

    [ObservableProperty]
    private List<AlertDto> _alerts = new();


    // Constructor that initializes the recommendation service and sets the title of the ViewModel.
    public RecommendationsViewModel(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
        Title = "Recommendations";
    }

    // This method loads the recommendations and alerts data from the service.
    // It checks for connectivity before making the API calls and updates the Recommendations and Alerts properties with the retrieved data.
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;

        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;

        try
        {
            IsBusy = true;

            var recommendationsTask = _recommendationService.GetRecommendationsAsync(); // Start fetching recommendations and alerts in parallel to improve performance.
            var alertsTask = _recommendationService.GetAlertsAsync();

            await Task.WhenAll(recommendationsTask, alertsTask); // Wait for both tasks to complete before updating the properties.

            Recommendations = recommendationsTask.Result;
            Alerts = alertsTask.Result;
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This method allows users to mark an alert as read.
    // It checks if the ViewModel is busy before proceeding, and then calls the service to mark the alert as read.
    [RelayCommand]
    private async Task MarkAlertAsReadAsync(AlertDto alert)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var success = await _recommendationService.MarkAlertAsReadAsync(alert.Id); // Call the service to mark the alert as read and check if it was successful.

            if (success)
                await LoadDataAsync();
            else
                await AlertHelper.ShowErrorAsync("Failed to mark alert as read.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

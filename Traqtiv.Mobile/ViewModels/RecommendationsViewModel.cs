using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
    private ObservableCollection<RecommendationDto> _recommendations = new();

    [ObservableProperty]
    private ObservableCollection<AlertDto> _alerts = new();

    // Constructor that initializes the recommendation service and sets the title of the ViewModel.
    public RecommendationsViewModel(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
        Title = "Recommendations";
    }

    // This method loads the recommendations and alerts data from the service.
    // It checks for connectivity before making the API calls and updates the Recommendations and Alerts properties with the retrieved data.
    [RelayCommand]
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

            // Clear existing data and update with the new data from the service.

            var recommendations = recommendationsTask.Result.Where(r => !r.IsRead).ToList();
            Recommendations.Clear();
            foreach (var r in recommendations)
                Recommendations.Add(r);


            var alerts = alertsTask.Result.Where(a => !a.IsRead).ToList();
            Alerts.Clear();
            foreach (var a in alerts)
                Alerts.Add(a);
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
                Alerts.Remove(alert);
            else
                await AlertHelper.ShowErrorAsync("Failed to mark alert as read.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // This method allows users to mark all alerts as read.
    [RelayCommand]
    private async Task MarkAllAlertsAsReadAsync()
    {
        if (IsBusy)
            return;


        var isConnected = await ConnectivityHelper.CheckAndAlertAsync();

        if (!isConnected) 
            return;

        try
        {
            IsBusy = true;
            var unreadAlerts = Alerts.Where(a => !a.IsRead).ToList(); // Get all unread alerts to mark them as read.
            foreach (var alert in unreadAlerts)
                await _recommendationService.MarkAlertAsReadAsync(alert.Id);
            
            
            var unreadRecommendations = Recommendations.Where(r => !r.IsRead).ToList(); // Get all unread recommendations to mark them as read.
            foreach (var recommendation in unreadRecommendations)
                await _recommendationService.MarkRecommendationAsReadAsync(recommendation.Id);

            Alerts.Clear();
            Recommendations.Clear();
        }
        finally
        {
            IsBusy = false;
        }
    }



    // This method allows users to mark a recommendation as read.
    [RelayCommand]
    private async Task MarkRecommendationAsReadAsync(RecommendationDto recommendation)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var success = await _recommendationService.MarkRecommendationAsReadAsync(recommendation.Id);
            if (success)
                Recommendations.Remove(recommendation);
            else
                await AlertHelper.ShowErrorAsync("Failed to mark recommendation as read.");
        }
        finally
        {
            IsBusy = false;
        }
    }








}


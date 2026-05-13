using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Traqtiv.Mobile.Helpers;
using Traqtiv.Mobile.Models;
using Traqtiv.Mobile.Services;
using Traqtiv.Mobile.Services.Interfaces;

namespace Traqtiv.Mobile.ViewModels;

//This ViewModel manages the body metrics data, allowing users to view their historical metrics and add new entries. It interacts with the IBodyMetricsService to fetch and save data, and also retrieves the latest health data from the HealthService to pre-fill the input fields for convenience.
//The ViewModel includes error handling and user feedback mechanisms to ensure a smooth user experience.

public partial class BodyMetricsViewModel : BaseViewModel
{
    private readonly IBodyMetricsService _metricsService;
    private readonly HealthService _healthService;

    [ObservableProperty]
    private List<BodyMetricsDto> _metricsList = new();

    [ObservableProperty]
    private double _weight;

    [ObservableProperty]
    private int _restingHeartRate;

    [ObservableProperty]
    private double _bmi;


    // Constructor that initializes the services and sets the title of the ViewModel.
    public BodyMetricsViewModel(IBodyMetricsService metricsService,HealthService healthService)
    {
        _metricsService = metricsService;
        _healthService = healthService;
        Title = "Body Metrics";
    }


    // This method loads the body metrics data from the service. It checks for connectivity before making the API call and updates the MetricsList property with the retrieved data.
    // It also fetches the latest health data to pre-fill the input fields for weight, resting heart rate, and BMI.
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy)
            return;

        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;
        // Set IsBusy to true to indicate that data is being loaded, and reset it to false in the finally block to ensure it happens regardless of success or failure.
        try
        {
            IsBusy = true;
            MetricsList = await _metricsService.GetMetricsAsync();

            
            var healthData = await _healthService.GetLatestMetricsAsync();
            if (healthData != null)
            {
                Weight = healthData.Weight;
                RestingHeartRate = healthData.RestingHeartRate;
                BMI = healthData.BMI;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }


    // This method saves the new body metrics data entered by the user.
    // It validates the input, checks for connectivity, and then sends the data to the service to be saved. It provides feedback to the user based on the success or failure of the operation.
    [RelayCommand]
    private async Task SaveMetricsAsync()
    {
        if (IsBusy)
            return;

        if (Weight <= 0)
        {
            await AlertHelper.ShowErrorAsync("Please enter a valid weight.");
            return;
        }

        if (!await ConnectivityHelper.CheckAndAlertAsync())
            return;


        // Set IsBusy to true to indicate that the save operation is in progress, and reset it to false in the finally block to ensure it happens regardless of success or failure.
        try
        {
            IsBusy = true;

            var request = new AddMetricsDto
            {
                Weight = Weight,
                RestingHeartRate = RestingHeartRate,
                BMI = BMI
            };

            var success = await _metricsService.AddMetricsAsync(request); // Call the service to save the new metrics data and store the result in a variable to determine if the operation was successful.

            if (success)
            {
                await AlertHelper.ShowSuccessAsync("Metrics saved successfully!");
                await LoadDataAsync();
            }
            else
                await AlertHelper.ShowErrorAsync("Failed to save metrics.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
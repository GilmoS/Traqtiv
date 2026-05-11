
namespace Traqtiv.Mobile.Helpers;

public static class AlertHelper
{
    // helper methods to show alerts in the app

    // showing an error alert with a message
    public static async Task ShowErrorAsync(string message)
    {
        await Shell.Current.DisplayAlert("Error", message, "OK");
    }

    // showing a success alert with a message
    public static async Task ShowSuccessAsync(string message)
    {
        await Shell.Current.DisplayAlert("Success", message, "OK");
    }
    // showing a confirmation dialog with a title and message, returning true if the user confirms (clicks "Yes") and false if they cancel (clicks "No")
    public static async Task<bool> ShowConfirmAsync(string title, string message)
    {
        return await Shell.Current.DisplayAlert(title, message, "Yes", "No");
    }
}

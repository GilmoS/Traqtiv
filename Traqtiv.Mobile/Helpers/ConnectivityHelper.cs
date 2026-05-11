namespace Traqtiv.Mobile.Helpers;

public static class ConnectivityHelper
{
    // helper methods to check internet connectivity in the app

    // checking if the device has an active internet connection
    public static bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }

    // checking if the device has an active internet connection and showing an alert
    // if not, returning true if connected and false if not
    public static async Task<bool> CheckAndAlertAsync()
    {
        if (!IsConnected())
        {
            await AlertHelper.ShowErrorAsync("No internet connection. Please check your connection and try again.");
            return false;
        }
        return true;
    }
}

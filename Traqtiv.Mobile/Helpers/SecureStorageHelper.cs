namespace Traqtiv.Mobile.Helpers;

public static class SecureStorageHelper
{
    // saving the JWT token securely on the device
    public static async Task SaveTokenAsync(string token)
    {
        await SecureStorage.SetAsync(AppConstants.TokenKey, token);
    }

    // reading the JWT token from secure storage
    public static async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(AppConstants.TokenKey);
    }

    // deleting the JWT token from secure storage (on logout)
    public static void RemoveToken()
    {
        SecureStorage.Remove(AppConstants.TokenKey);
    }

    // checking if the user is logged in by verifying if a token exists in secure storage
    public static async Task<bool> IsLoggedInAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
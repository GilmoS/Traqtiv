namespace Traqtiv.Mobile.Helpers;


// This helper class provides utility methods for retrieving the user's current geographic location and converting it into a human-readable city name.
// It handles permission requests for location access and uses the Geolocation and Geocoding APIs to obtain the necessary data.
public static class LocationHelper
{
    // This method retrieves the user's current geographic location (latitude and longitude) asynchronously.
    public static async Task<(double latitude, double longitude)?> GetCurrentLocationAsync()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await AlertHelper.ShowErrorAsync("Location permission is required for weather data.");
                return null;
            }

            var location = await Geolocation.Default.GetLocationAsync(
                new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

            if (location != null)
                return (location.Latitude, location.Longitude);

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }


    // This method takes latitude and longitude as input and returns a human-readable city name along with the country.
    // It uses the Geocoding API to convert geographic coordinates into a placemark, from which it extracts the locality (city) and country name.
    public static async Task<string> GetCityNameAsync(double latitude, double longitude)
    {
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var city = placemark.Locality ?? placemark.AdminArea ?? "";
                var country = placemark.CountryName ?? "";
                return $"{city}, {country}";
            }
            return "Unknown location";
        }
        catch (Exception)
        {
            return "Unknown location";
        }
    }
}

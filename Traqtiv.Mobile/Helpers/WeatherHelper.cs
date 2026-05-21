namespace Traqtiv.Mobile.Helpers;


//this helper class provides utility methods for interpreting weather data, such as converting weather descriptions to emoji icons and translating air quality index values into human-readable descriptions with corresponding emojis.
public static class WeatherHelper
{
    
    //This method takes a weather description string and returns an appropriate emoji icon representing the weather condition.
    public static string GetWeatherIcon(string description)
    {
        return description.ToLower() switch
        {
            var d when d.Contains("sun") || d.Contains("clear") => "☀️",
            var d when d.Contains("cloud") => "☁️",
            var d when d.Contains("rain") => "🌧️",
            var d when d.Contains("snow") => "❄️",
            var d when d.Contains("storm") || d.Contains("thunder") => "⛈️",
            var d when d.Contains("fog") || d.Contains("mist") => "🌫️",
            _ => "🌤️"
        };
    }


    public static string GetOutdoorRecommendation(double temperature, int aqi)
    {
        if (aqi >= 4)
            return "⚠️ Poor air quality. Indoor workout recommended.";
        if (temperature > 35)
            return "🌡️ Too hot for outdoor workout. Stay hydrated!";
        if (temperature < 5)
            return "🥶 Too cold outside. Consider indoor workout.";

        return "🏃 Great conditions for outdoor workout!";
    }
}
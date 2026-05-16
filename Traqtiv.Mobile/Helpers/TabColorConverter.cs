using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

// This converter is used to change the color of the tab icons in the BottomNavBar based on whether they are active or not.
public class TabColorConverter : IValueConverter
{
    // The Convert method takes the active tab and the tab associated with the icon as parameters and returns the appropriate color.
    public object Convert(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        var activeTab = value?.ToString();
        var tab = parameter?.ToString();
        return activeTab == tab? Color.FromArgb("#378ADD"): Color.FromArgb("#5F5E5A");// Active tab gets a bright blue color, while inactive tabs get a gray color.
    }

    // The ConvertBack method is not implemented because we only need one-way conversion for this use case.
    public object ConvertBack(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
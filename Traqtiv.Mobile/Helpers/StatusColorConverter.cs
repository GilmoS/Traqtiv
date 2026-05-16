using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

// The StatusColorConverter class is a value converter that implements the IValueConverter interface.
public class StatusColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        var selected = value?.ToString();
        var param = parameter?.ToString();
        return selected == param? Color.FromArgb("#1D9E7520"): Color.FromArgb("#1A2634"); // Return the color based on whether the selected value matches the parameter, using specific colors for each case.
    }

    // The ConvertBack method is not implemented in this converter, as it is not needed for the intended use case.
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
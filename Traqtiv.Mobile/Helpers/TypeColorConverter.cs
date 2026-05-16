using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

// The TypeColorConverter class is a value converter that implements the IValueConverter interface.
// It is used to convert a selected value and a parameter into a specific color based on whether they match or not.
public class TypeColorConverter : IValueConverter
{
    // The Convert method takes a value, target type, parameter, and culture information to determine the appropriate color to return based on the selected value and the parameter.
    public object Convert(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        var selected = value?.ToString();
        var param = parameter?.ToString();
        return selected == param? Color.FromArgb("#378ADD"): Color.FromArgb("#1A2634"); // Return the color based on whether the selected value matches the parameter, using specific colors for each case.
    }

    // The ConvertBack method is not implemented in this converter, as it is not needed for the intended use case. 
    public object ConvertBack(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
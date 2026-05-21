using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

//this converter class is used in the XAML bindings to convert string values to boolean values,
//for controlling the visibility or enabled state of UI elements based on whether a string is null or empty.
public class StringToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrEmpty(value?.ToString());
    }

    public object ConvertBack(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
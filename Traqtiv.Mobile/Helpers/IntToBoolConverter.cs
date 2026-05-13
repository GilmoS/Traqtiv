using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

// This class is a value converter that converts an integer value to a boolean.
public class IntToBoolConverter : IValueConverter
{
    // The Convert method takes an integer value and returns true if the value is greater than 0, otherwise it returns false.
    public object Convert(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        if (value is int count)
            return count > 0;
        return false;
    }

    // The ConvertBack method is not implemented in this converter, as it is not needed for the intended use case.
    public object ConvertBack(object? value, Type targetType,object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
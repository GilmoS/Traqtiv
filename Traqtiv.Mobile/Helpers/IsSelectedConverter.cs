using System.Globalization;

namespace Traqtiv.Mobile.Helpers;

// Compares the bound SelectedWorkout (value) against the current row's item (parameter)
// to control the visibility of the expanded details section in WorkoutsPage.
public class IsSelectedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
            return false;

        return value.Equals(parameter);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

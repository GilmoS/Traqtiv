namespace Traqtiv.API.Helpers
{
    /// Provides common helpers for computing date boundaries and simple date checks.
    public static class DateTimeHelper
    {
        /// Returns the start of the week for the specified date.
        /// This method treats Monday as the first day of the week.
        public static DateTime StartOfWeek(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        /// Returns the end of the week for the specified date.
        /// The end of week is computed as the start of the week plus six days.
        public static DateTime EndOfWeek(DateTime date)
        {
            return StartOfWeek(date).AddDays(6);
        }

        /// Returns the first day of the month for the specified date.
        public static DateTime StartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// Returns the last day of the month for the specified date.
        public static DateTime EndOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month,
                DateTime.DaysInMonth(date.Year, date.Month));
        }
        /// Determines whether the specified date is within the last days.
        public static bool IsWithinDays(DateTime date, int days)
        {
            return (DateTime.UtcNow - date).TotalDays <= days;
        }
    }
}

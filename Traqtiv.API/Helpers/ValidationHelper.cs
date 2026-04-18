using System.Text.RegularExpressions;

namespace Traqtiv.API.Helpers
{
    /// Provides simple validation helpers for common user input values.

    public static class ValidationHelper
    {
        /// Determines whether the specified string is a syntactically valid email address.
        /// This uses a simple regular expression to perform a syntactic check. It does not guarantee the address exists or is deliverable.
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }
        /// Determines whether the specified password meets the minimum requirements.
        /// Minimum requirements are intentionally simple here; will adjust rules (complexity, special characters) if policy changes.
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }
        /// Determines whether the specified name is valid.
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length >= 2;
        }
    }
}

namespace Traqtiv.API.Helpers
{

    /// Provides helper methods for hashing and verifying passwords using BCrypt.
    public static class PasswordHelper
    {
        /// Hashes the specified plaintext password using BCrypt.
        /// This method delegates to BCrypt.Net.BCrypt.HashPassword.
        /// Ensure the returned hash is stored securely (never store plaintext passwords).
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        /// Verifies a plaintext password against a previously hashed password.
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

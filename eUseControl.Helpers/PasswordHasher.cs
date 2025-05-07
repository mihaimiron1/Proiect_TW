using System;
using BCrypt.Net;

namespace eUseControl.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a salt and hash the password
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Verify the password against the hash
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 
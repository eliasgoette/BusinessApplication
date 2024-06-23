using System.Security.Cryptography;

namespace BusinessApplication.Utility
{
    public class PasswordEncryption
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public static string HashPassword(string clearText)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                clearText,
                salt,
                Iterations,
                HashAlgorithm,
                KeySize
            );

            byte[] hashBytes = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool MatchPassword(string passwordAttempt, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] storedPasswordHash = new byte[KeySize];
            Array.Copy(hashBytes, SaltSize, storedPasswordHash, 0, KeySize);

            byte[] hashAttempt = Rfc2898DeriveBytes.Pbkdf2(
                passwordAttempt,
                salt,
                Iterations,
                HashAlgorithm,
                KeySize
            );

            return CryptographicOperations.FixedTimeEquals(storedPasswordHash, hashAttempt);
        }
    }
}

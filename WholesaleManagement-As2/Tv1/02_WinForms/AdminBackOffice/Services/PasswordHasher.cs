using System;
using System.Security.Cryptography;
using System.Text;

namespace AdminBackOffice.Services
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required.");
            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create()) random.GetBytes(salt);
            const int iterations = 120000;
            using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return "PBKDF2$" + iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(kdf.GetBytes(32));
            }
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash)) return false;
            try
            {
                var parts = storedHash.Split('$');
                if (parts.Length != 4 || parts[0] != "PBKDF2") return false;
                int iterations;
                if (!int.TryParse(parts[1], out iterations) || iterations < 100000 || iterations > 1000000) return false;
                var salt = Convert.FromBase64String(parts[2]);
                var expected = Convert.FromBase64String(parts[3]);
                if (salt.Length != 16 || expected.Length != 32) return false;
                using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    var actual = kdf.GetBytes(expected.Length);
                    var diff = 0;
                    for (var i = 0; i < actual.Length; i++) diff |= actual[i] ^ expected[i];
                    return diff == 0;
                }
            }
            catch (FormatException) { return false; }
            catch (ArgumentException) { return false; }
        }
    }
}

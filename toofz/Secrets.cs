using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace toofz
{
    /// <summary>
    /// Provides methods for encrypting and decrypting secrets.
    /// </summary>
    static class Secrets
    {
        const int SaltSize = 8;

        // Using the host's physical address is a compromise between security and ease of use.
        // One of the goals of the toofz projects is for the code to be easy to evaluate and understand.
        // Making it straight-forward to run projects is a step in this direction, however, it does mean that 
        // we can't ask users to secure a password.
        static readonly string Password = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                                           where nic.OperationalStatus == OperationalStatus.Up
                                           select nic.GetPhysicalAddress())
                                          .FirstOrDefault()
                                          .ToString();

        /// <summary>
        /// Encrypts a secret.
        /// </summary>
        /// <param name="secret">The secret to encrypt.</param>
        /// <param name="iterations">The number of iterations for the operation.</param>
        /// <returns>
        /// The encrypted secret and its associated salt.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="secret"/> cannot be null or empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="iterations"/> is out of range. This parameter requires a non-negative number.
        /// </exception>
        public static (byte[] encrypted, byte[] salt) Encrypt(string secret, int iterations)
        {
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentException($"{nameof(secret)} cannot be null or empty.", nameof(secret));

            using (var pbkdf2 = new Rfc2898DeriveBytes(Password, SaltSize, iterations))
            using (var alg = Rijndael.Create())
            {
                alg.Key = pbkdf2.GetBytes(32);
                alg.IV = pbkdf2.GetBytes(16);

                using (var inStream = new MemoryStream(Encoding.UTF8.GetBytes(secret)))
                using (var outStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(outStream, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inStream.CopyTo(cryptoStream);
                    cryptoStream.FlushFinalBlock();

                    var encrypted = outStream.ToArray();
                    var salt = pbkdf2.Salt;

                    return (encrypted, salt);
                }
            }
        }

        /// <summary>
        /// Decrypts a secret.
        /// </summary>
        /// <param name="encrypted">The encrypted secret.</param>
        /// <param name="salt">The encrypted secret's associated salt.</param>
        /// <param name="iterations">The number of iterations for the operation.</param>
        /// <returns>
        /// The decrypted secret.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encrypted"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="salt"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="iterations"/> is out of range. This parameter requires a non-negative number.
        /// </exception>
        public static string Decrypt(byte[] encrypted, byte[] salt, int iterations)
        {
            if (encrypted == null)
                throw new ArgumentNullException(nameof(encrypted));
            if (salt == null)
                throw new ArgumentNullException(nameof(salt));

            using (var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, iterations))
            using (var alg = Rijndael.Create())
            {
                alg.Key = pbkdf2.GetBytes(32);
                alg.IV = pbkdf2.GetBytes(16);

                using (var inStream = new MemoryStream(encrypted))
                using (var outStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(outStream, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    inStream.CopyTo(cryptoStream);
                    cryptoStream.FlushFinalBlock();
                    outStream.Position = 0;

                    using (var sr = new StreamReader(outStream))
                    {
                        var decrypted = sr.ReadToEnd();

                        return decrypted;
                    }
                }
            }
        }
    }
}
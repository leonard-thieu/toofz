using System;

namespace toofz
{
    public sealed class EncryptedSecret
    {
        // Required for XML serialization
        EncryptedSecret() { }

        /// <summary>
        /// Initializes an instance of the <see cref="EncryptedSecret"/> class.
        /// </summary>
        /// <param name="secret">The secret to encrypt.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="secret"/> cannot be null or empty.
        /// </exception>
        public EncryptedSecret(string secret)
        {
            (Secret, Salt) = Secrets.Encrypt(secret);
        }

        // Secret and Salt are exposed as public byte[] due to XML serialization requirements.

        byte[] secret;
        public byte[] Secret
        {
            get => secret;
            set => secret = value ?? throw new ArgumentNullException(nameof(value));
        }

        byte[] salt;
        public byte[] Salt
        {
            get => salt;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length < 8)
                    throw new ArgumentException($"The size of the salt must be at least 8 bytes.");

                salt = value;
            }
        }

        /// <summary>
        /// Decrypts the encrypted secret.
        /// </summary>
        /// <returns>
        /// The decrypted secret.
        /// </returns>
        public string Decrypt()
        {
            return Secrets.Decrypt(Secret, Salt);
        }
    }
}

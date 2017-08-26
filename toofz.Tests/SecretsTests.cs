using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.Tests
{
    class SecretsTests
    {
        [TestClass]
        public class Encrypt
        {
            [TestMethod]
            public void SecretIsNull_ThrowsArgumentException()
            {
                // Arrange
                string secret = null;
                int iterations = 1000;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    Secrets.Encrypt(secret, iterations);
                });
            }

            [TestMethod]
            public void SecretIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string secret = "";
                int iterations = 1000;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    Secrets.Encrypt(secret, iterations);
                });
            }

            [TestMethod]
            public void IterationsIsNegative_ThrowsArgumentOutOfRangeException()
            {
                // Arrange
                string secret = "mySecret";
                int iterations = -1;

                // Act -> Assert
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    Secrets.Encrypt(secret, iterations);
                });
            }

            [TestMethod]
            public void ReturnsEncryptedSecret()
            {
                // Arrange
                string secret = "mySecret";
                int iterations = 1000;

                // Act
                var (encrypted, salt) = Secrets.Encrypt(secret, iterations);

                // Assert
                Assert.IsTrue(encrypted.Any());
            }

            [TestMethod]
            public void ReturnsSalt()
            {
                // Arrange
                string secret = "mySecret";
                int iterations = 1000;

                // Act
                var (encrypted, salt) = Secrets.Encrypt(secret, iterations);

                // Assert
                Assert.IsTrue(salt.Any());
            }
        }

        [TestClass]
        public class Decrypt
        {
            [TestMethod]
            public void EncryptedIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                byte[] encrypted = null;
                byte[] salt = new byte[8];
                int iterations = 1000;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    Secrets.Decrypt(encrypted, salt, iterations);
                });
            }

            [TestMethod]
            public void SaltIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                byte[] encrypted = new byte[8];
                byte[] salt = null;
                int iterations = 1000;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    Secrets.Decrypt(encrypted, salt, iterations);
                });
            }

            [TestMethod]
            public void SaltIsLessThan8Bytes_ThrowsArgumentException()
            {
                // Arrange
                byte[] encrypted = new byte[8];
                byte[] salt = new byte[0];
                int iterations = 1000;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    Secrets.Decrypt(encrypted, salt, iterations);
                });
            }

            [TestMethod]
            public void IterationsIsNegative_ThrowsArgumentOutOfRangeException()
            {
                // Arrange
                byte[] encrypted = new byte[8];
                byte[] salt = new byte[8];
                int iterations = -1;

                // Act -> Assert
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    Secrets.Decrypt(encrypted, salt, iterations);
                });
            }

            [TestMethod]
            public void ReturnsDecryptedSecret()
            {
                // Arrange
                var secret = "mySecret";
                int iterations = 1000;
                var (encrypted, salt) = Secrets.Encrypt(secret, iterations);

                // Act
                var decrypted = Secrets.Decrypt(encrypted, salt, iterations);

                // Assert
                Assert.AreEqual(secret, decrypted);
            }
        }
    }
}

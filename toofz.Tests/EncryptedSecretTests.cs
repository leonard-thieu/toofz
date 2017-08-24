﻿using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.Tests
{
    class EncryptedSecretTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void SecretIsNull_ThrowsArgumentException()
            {
                // Arrange
                string secret = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new EncryptedSecret(secret);
                });
            }

            [TestMethod]
            public void SecretIsEmpty_ThrowsArgumentException()
            {
                // Arrange
                string secret = "";

                // Act -> Assert
                Assert.ThrowsException<ArgumentException>(() =>
                {
                    new EncryptedSecret(secret);
                });
            }

            [TestMethod]
            public void ReturnsEncryptedSecret()
            {
                // Arrange
                string secret = "mySecret";

                // Act
                var encryptedSecret = new EncryptedSecret(secret);

                // Assert
                Assert.IsInstanceOfType(encryptedSecret, typeof(EncryptedSecret));
            }
        }

        [TestClass]
        public class Decrypt
        {
            [TestMethod]
            public void ReturnsDecryptedSecret()
            {
                // Arrange
                string secret = "mySecret";
                var encryptedSecret = new EncryptedSecret(secret);

                // Act
                var decryptedSecret = encryptedSecret.Decrypt();

                // Assert
                Assert.AreEqual(secret, decryptedSecret);
            }
        }

        [TestClass]
        public class Serialization
        {
            [TestMethod]
            public void SerializesAndDeserializes()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                var sw = new StringWriter();
                provider.GetSettingsWriter = () => sw;
                var context = new SettingsContext();
                var values = new SettingsPropertyValueCollection();
                var property = SettingsUtil.CreateProperty<EncryptedSecret>("myProp");
                property.SerializeAs = SettingsSerializeAs.Xml;
                var value = new SettingsPropertyValue(property);
                var encryptedSecret = new EncryptedSecret("mySecret");
                value.PropertyValue = encryptedSecret;
                values.Add(value);

                // Act
                provider.SetPropertyValues(context, values);

                // Arrange
                provider.GetSettingsReader = () => new StringReader(sw.ToString());
                var properties = new SettingsPropertyCollection();
                properties.Add(property);

                // Act
                var values2 = provider.GetPropertyValues(context, properties);
                var myProp = values2["myProp"].PropertyValue;

                // Assert
                Assert.IsInstanceOfType(myProp, typeof(EncryptedSecret));
                var encryptedSecret2 = (EncryptedSecret)myProp;
                Assert.AreEqual("mySecret", encryptedSecret2.Decrypt());
            }
        }
    }
}

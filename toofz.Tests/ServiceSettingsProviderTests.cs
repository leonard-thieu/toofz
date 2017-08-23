using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.Tests.Properties;
using toofz.TestsShared;

namespace toofz.Tests
{
    class ServiceSettingsProviderTests
    {
        [TestClass]
        public class ApplicationName
        {
            [TestMethod]
            public void SetToNull_ThrowsArgumentNullException()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                string applicationName = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    provider.ApplicationName = applicationName;
                });
            }

            [TestMethod]
            public void ReturnsADefaultValue()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();

                // Act
                var applicationName = provider.ApplicationName;

                // Assert
                Assert.AreEqual("toofz", applicationName);
            }

            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                string applicationName = "My Application";

                // Act
                provider.ApplicationName = applicationName;

                // Assert
                Assert.AreEqual(applicationName, provider.ApplicationName);
            }
        }

        [TestClass]
        public class GetSettingsStream
        {
            [TestMethod]
            public void SetToNull_ThrowsArgumentNullException()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                Func<Stream> getSettingsStream = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    provider.GetSettingsStream = getSettingsStream;
                });
            }

            [TestMethod]
            public void ReturnsADefaultValue()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();

                // Act
                var getSettingsStream = provider.GetSettingsStream;

                // Assert
                Assert.IsNotNull(getSettingsStream);
            }

            [TestMethod]
            public void GetSetBehavior()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                Func<Stream> getSettingsStream = () => new MemoryStream();

                // Act
                provider.GetSettingsStream = getSettingsStream;

                // Assert
                Assert.AreEqual(getSettingsStream, provider.GetSettingsStream);
            }
        }

        [TestClass]
        public class Initialize
        {
            [TestMethod]
            public void NameIsNull_DoesNotThrowArgumentNullException()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();

                // Act -> Assert
                provider.Initialize(null, new NameValueCollection());
            }

            [TestMethod]
            public void Initializes()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();

                // Act
                provider.Initialize("myName", new NameValueCollection());

                // Assert
                Assert.AreEqual(provider.ApplicationName, provider.Name);
                Assert.AreEqual(provider.ApplicationName, provider.Description);
            }
        }

        [TestClass]
        public class GetPropertyValues
        {
            [TestMethod]
            public void ReturnsValuesFromConfig()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                provider.GetSettingsStream = () => new MemoryStream(Encoding.UTF8.GetBytes(Resources.UserConfig));
                var context = new SettingsContext();
                var properties = new SettingsPropertyCollection();
                var property1 = new SettingsProperty("myProp1");
                properties.Add(property1);
                var property2 = new SettingsProperty("myProp2");
                properties.Add(property2);

                // Act
                var values = provider.GetPropertyValues(context, properties);

                // Assert
                Assert.AreEqual(2, values.Count);
                Assert.AreEqual("mySerializedValue1", values["myProp1"].SerializedValue);
                Assert.AreEqual("mySerializedValue2", values["myProp2"].SerializedValue);
            }
        }

        [TestClass]
        public class SetPropertyValues
        {
            [TestMethod]
            public void SetsValuesInConfig()
            {
                // Arrange
                var provider = new ServiceSettingsProvider();
                var ms = new UndisposableMemoryStream();
                provider.GetSettingsStream = () => ms;
                var context = new SettingsContext();
                var values = new SettingsPropertyValueCollection();
                var property1 = new SettingsProperty("myProp1");
                var value1 = new SettingsPropertyValue(property1) { SerializedValue = "mySerializedValue1" };
                values.Add(value1);
                var property2 = new SettingsProperty("myProp2");
                var value2 = new SettingsPropertyValue(property2) { SerializedValue = "mySerializedValue2" };
                values.Add(value2);

                // Act
                provider.SetPropertyValues(context, values);

                // Assert
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    AssertHelper.NormalizedAreEqual(Resources.UserConfig, sr.ReadToEnd());
                }
            }
        }
    }
}

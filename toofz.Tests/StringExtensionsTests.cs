using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.Tests
{
    class StringExtensionsTests
    {
        [TestClass]
        public class ToStream
        {
            [TestMethod]
            public void ValueIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string value = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    value.ToStream();
                });
            }

            [TestMethod]
            public void ReturnsValueAsStream()
            {
                // Arrange
                var value = "myValue";

                // Act
                var stream = value.ToStream();

                // Assert
                using (var sr = new StreamReader(stream))
                {
                    Assert.AreEqual(value, sr.ReadToEnd());
                }
            }
        }
    }
}

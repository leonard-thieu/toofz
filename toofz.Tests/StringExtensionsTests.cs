using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

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

                // Act
                var ex = Record.Exception(() =>
                {
                    value.ToStream();
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
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

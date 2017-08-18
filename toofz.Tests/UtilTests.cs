using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class UtilTests
    {
        [TestClass]
        public class GetEnvVar
        {
            [TestMethod]
            public void EnvironmentVariableIsNotSet_ThrowsArgumentNullException()
            {
                // Arrange
                var variable = "myVariable";
                var mockEnvironment = new Mock<IEnvironment>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    Util.GetEnvVar(variable, mockEnvironment.Object);
                });
            }

            [TestMethod]
            public void ReturnsEnvironmentVariable()
            {
                // Arrange
                var variable = "myVariable";
                var mockEnvironment = new Mock<IEnvironment>();
                mockEnvironment
                    .Setup(environment => environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine))
                    .Returns("myValue");

                // Act
                var value = Util.GetEnvVar(variable, mockEnvironment.Object);

                // Assert
                Assert.AreEqual("myValue", value);
            }
        }
    }
}

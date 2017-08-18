using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.TestsShared;

namespace toofz.Tests
{
    class ActionProgressTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void HandlerIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                Action<long> handler = null;

                // Act
                var ex = Record.Exception(() =>
                {
                    new ActionProgress<long>(handler);
                });

                // Assert
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                Action<long> handler = progress => { };

                // Act
                var actionProgress = new ActionProgress<long>(handler);

                // Assert
                Assert.IsInstanceOfType(actionProgress, typeof(ActionProgress<long>));
            }
        }

        [TestClass]
        public class Report
        {
            [TestMethod]
            public void CallsHandlerWithValue()
            {
                // Arrange
                var hasBeenCalled = false;
                var value = 20;
                Action<long> handler = progress =>
                {
                    Assert.AreEqual(value, progress);
                    hasBeenCalled = true;
                };
                var actionProgress = new ActionProgress<long>(handler);

                // Act
                actionProgress.Report(value);

                // Assert
                Assert.IsTrue(hasBeenCalled);
            }
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new ActionProgress<long>(handler);
                });
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

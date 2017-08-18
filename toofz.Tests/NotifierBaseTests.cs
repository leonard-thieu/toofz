using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class NotifierBaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void CategoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string category = null;
                var mockLog = new Mock<ILog>();
                var name = "myName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new MockNotifierBase(category, mockLog.Object, name);
                });
            }

            [TestMethod]
            public void LogIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                ILog log = null;
                var name = "myName";

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new MockNotifierBase(category, log, name);
                });
            }

            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                string name = null;

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new MockNotifierBase(category, mockLog.Object, name);
                });
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var name = "myName";

                // Act
                var notifier = new MockNotifierBase(category, mockLog.Object, name);

                // Assert
                Assert.IsInstanceOfType(notifier, typeof(NotifierBase));
            }

            [TestMethod]
            public void LogsStartMessage()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var name = "myName";

                // Act
                var notifier = new MockNotifierBase(category, mockLog.Object, name);

                // Assert
                mockLog.Verify(log => log.Debug("Start myCategory myName"));
            }
        }

        [TestClass]
        public class Dispose
        {
            [TestMethod]
            public void LogsEndMessage()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var name = "myName";
                var notifier = new MockNotifierBase(category, mockLog.Object, name);

                // Act
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Debug("End myCategory myName"));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsEndMessageOnce()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var name = "myName";
                var notifier = new MockNotifierBase(category, mockLog.Object, name);

                // Act
                notifier.Dispose();
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Debug("End myCategory myName"), Times.Once);
            }
        }
    }
}

using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class StoreNotifierTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "myName";

                // Act
                var notifier = new StoreNotifier(mockLog.Object, name);

                // Assert
                Assert.IsInstanceOfType(notifier, typeof(StoreNotifier));
            }
        }

        [TestClass]
        public class RowsAffectedProperty
        {
            [TestMethod]
            public void ReturnsRowsAffected()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";
                var notifier = new StoreNotifier(log, name);
                notifier.Report(20);

                // Act
                var rowsAffected = notifier.RowsAffected;

                // Assert
                Assert.AreEqual(20, rowsAffected);
            }
        }

        [TestClass]
        public class ReportMethod
        {
            [TestMethod]
            public void AddsValueToRowsAffected()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";
                var notifier = new StoreNotifier(log, name);

                // Act
                notifier.Report(1);
                notifier.Report(1);

                // Assert
                Assert.AreEqual(2, notifier.RowsAffected);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void LogsCompletionMessage()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "entries";
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new StoreNotifier(mockLog.Object, name, mockStopwatch.Object);
                notifier.Report(759225);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((6.42).Seconds());

                // Act
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "entries";
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new StoreNotifier(mockLog.Object, name, mockStopwatch.Object);
                notifier.Report(759225);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((6.42).Seconds());

                // Act
                notifier.Dispose();
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."), Times.Once);
            }
        }
    }
}

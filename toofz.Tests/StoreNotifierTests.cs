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
            public void SetsProgress()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "myName";

                // Act
                var notifier = new StoreNotifier(mockLog.Object, name);

                // Assert
                Assert.IsNotNull(notifier.Progress);
            }
        }

        [TestClass]
        public class Dispose
        {
            [TestMethod]
            public void LogsCompletionMessage()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "entries";
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new StoreNotifier(mockLog.Object, name, mockStopwatch.Object);
                notifier.Progress.Report(759225);
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
                notifier.Progress.Report(759225);
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

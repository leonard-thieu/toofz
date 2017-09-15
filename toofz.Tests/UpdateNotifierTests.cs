using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class UpdateNotifierTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";

                // Act
                var notifier = new UpdateNotifier(log, name);

                // Assert
                Assert.IsInstanceOfType(notifier, typeof(UpdateNotifier));
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
                var name = "daily leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new UpdateNotifier(mockLog.Object, name, mockStopwatch.Object);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((13.2).Seconds());

                // Act
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Update daily leaderboards complete after 13.2 s."));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "daily leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new UpdateNotifier(mockLog.Object, name, mockStopwatch.Object);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((13.2).Seconds());

                // Act
                notifier.Dispose();
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Update daily leaderboards complete after 13.2 s."), Times.Once);
            }
        }
    }
}

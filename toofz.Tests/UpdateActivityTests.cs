using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    public class UpdateActivityTests
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
                var activity = new UpdateActivity(log, name);

                // Assert
                Assert.IsInstanceOfType(activity, typeof(UpdateActivity));
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
                var log = mockLog.Object;
                var name = "daily leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new UpdateActivity(log, name, stopwatch);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((13.2).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Update daily leaderboards complete after 13.2 s."));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "daily leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new UpdateActivity(log, name, stopwatch);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((13.2).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Update daily leaderboards complete after 13.2 s."), Times.Once);
            }
        }
    }
}

using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class UpdateActivityTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";

                // Act
                var activity = new UpdateActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<UpdateActivity>(activity);
            }
        }

        public class DisposeMethod
        {
            [Fact]
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

            [Fact]
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

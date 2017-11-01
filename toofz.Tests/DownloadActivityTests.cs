using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class DownloadActivityTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "leaderboards";

                // Act
                var activity = new DownloadActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<DownloadActivity>(activity);
            }
        }

        public class TotalBytesProperty
        {
            [Fact]
            public void ReturnsTotalBytes()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "leaderboards";
                var activity = new DownloadActivity(log, name);
                activity.Report(21);
                activity.Report(21);

                // Act
                var totalBytes = activity.TotalBytes;

                // Assert
                Assert.Equal(42, totalBytes);
            }
        }

        public class ReportMethod
        {
            [Fact]
            public void AddsValueToTotalBytes()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "myName";
                var activity = new DownloadActivity(log, name);

                // Act
                activity.Report(1);
                activity.Report(1);

                // Assert
                Assert.Equal(2, activity.TotalBytes);
            }
        }

        public class DisposeMethod
        {
            [Fact]
            public void LogsSizeTimeAndRate()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new DownloadActivity(log, name, stopwatch);
                activity.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((10.34).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."));
            }

            [Fact]
            public void DisposingMoreThanOnce_LogsSizeTimeAndRateOnlyOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "leaderboards";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new DownloadActivity(log, name, stopwatch);
                activity.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((10.34).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."), Times.Once);
            }
        }
    }
}

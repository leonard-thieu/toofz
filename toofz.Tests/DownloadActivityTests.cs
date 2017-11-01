using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    public class DownloadActivityTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var log = Mock.Of<ILog>();
                var name = "leaderboards";

                // Act
                var activity = new DownloadActivity(log, name);

                // Assert
                Assert.IsInstanceOfType(activity, typeof(DownloadActivity));
            }
        }

        [TestClass]
        public class TotalBytesProperty
        {
            [TestMethod]
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
                Assert.AreEqual(42, totalBytes);
            }
        }

        [TestClass]
        public class ReportMethod
        {
            [TestMethod]
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
                Assert.AreEqual(2, activity.TotalBytes);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
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

            [TestMethod]
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

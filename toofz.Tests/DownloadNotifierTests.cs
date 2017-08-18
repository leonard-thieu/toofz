using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class DownloadNotifierTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "leaderboards";

                // Act
                var notifier = new DownloadNotifier(mockLog.Object, name);

                // Assert
                Assert.IsInstanceOfType(notifier, typeof(DownloadNotifier));
                Assert.IsNotNull(notifier.Progress);
            }
        }

        [TestClass]
        public class TotalBytes
        {
            [TestMethod]
            public void ReturnsTotalBytes()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var name = "leaderboards";
                var notifier = new DownloadNotifier(mockLog.Object, name);
                notifier.Progress.Report(21);
                notifier.Progress.Report(21);

                // Act
                var totalBytes = notifier.TotalBytes;

                // Assert
                Assert.AreEqual(42, totalBytes);
            }
        }

        [TestClass]
        public class Dispose
        {
            [TestMethod]
            public void LogsSizeTimeAndRate()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new DownloadNotifier(mockLog.Object, "leaderboards", mockStopwatch.Object);
                notifier.Progress.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((10.34).Seconds());

                // Act
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_LogsSizeTimeAndRateOnlyOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var mockStopwatch = new Mock<IStopwatch>();
                var notifier = new DownloadNotifier(mockLog.Object, "leaderboards", mockStopwatch.Object);
                notifier.Progress.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((10.34).Seconds());

                // Act
                notifier.Dispose();
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."), Times.Once);
            }
        }
    }
}

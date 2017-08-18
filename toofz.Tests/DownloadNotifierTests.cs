using System;
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
        public class Dispose
        {
            Mock<ILog> mockLog;
            Mock<IStopwatch> mockStopwatch;
            DownloadNotifier notifier;
            IProgress<long> progress;

            [TestInitialize]
            public void Initialize()
            {
                mockLog = new Mock<ILog>();
                mockStopwatch = new Mock<IStopwatch>();
                notifier = new DownloadNotifier(mockLog.Object, "leaderboards", mockStopwatch.Object);
                progress = notifier.Progress;
            }

            [TestMethod]
            public void LogsSizeTimeAndRate()
            {
                // Arrange
                progress.Report((long)(26.3).Megabytes().Bytes);
                mockStopwatch
                    .SetupGet(stopwatch => stopwatch.Elapsed)
                    .Returns((10.34).Seconds());

                // Act
                notifier.Dispose();

                // Assert
                mockLog.Verify(log => log.Info("Download leaderboards complete -- 26.3 MB over 10.3 seconds (2.5 MBps)."));
            }
        }
    }
}

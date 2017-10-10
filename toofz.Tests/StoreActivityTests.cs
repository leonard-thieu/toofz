using Humanizer;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class StoreActivityTests
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
                var activity = new StoreActivity(log, name);

                // Assert
                Assert.IsInstanceOfType(activity, typeof(StoreActivity));
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
                var activity = new StoreActivity(log, name);
                activity.Report(20);

                // Act
                var rowsAffected = activity.RowsAffected;

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
                var activity = new StoreActivity(log, name);

                // Act
                activity.Report(1);
                activity.Report(1);

                // Assert
                Assert.AreEqual(2, activity.RowsAffected);
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
                var name = "entries";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new StoreActivity(log, name, stopwatch);
                activity.Report(759225);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((6.42).Seconds());

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsCompletionMessageOnce()
            {
                // Arrange
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "entries";
                var mockStopwatch = new Mock<IStopwatch>();
                var stopwatch = mockStopwatch.Object;
                var activity = new StoreActivity(log, name, stopwatch);
                activity.Report(759225);
                mockStopwatch.SetupGet(s => s.Elapsed).Returns((6.42).Seconds());

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Info("Store entries complete -- 759225 rows affected over 6.4 seconds (118259 rows per second)."), Times.Once);
            }
        }
    }
}

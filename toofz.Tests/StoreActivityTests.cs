using Humanizer;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class StoreActivityTests
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
                var activity = new StoreActivity(log, name);

                // Assert
                Assert.IsAssignableFrom<StoreActivity>(activity);
            }
        }

        public class RowsAffectedProperty
        {
            [Fact]
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
                Assert.Equal(20, rowsAffected);
            }
        }

        public class ReportMethod
        {
            [Fact]
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
                Assert.Equal(2, activity.RowsAffected);
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

            [Fact]
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

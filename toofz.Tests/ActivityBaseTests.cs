using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    public class ActivityBaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void CategoryIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                string category = null;
                var log = Mock.Of<ILog>();
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new ActivityBaseAdapter(category, log, name, stopwatch);
                });
            }

            [TestMethod]
            public void LogIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                ILog log = null;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new ActivityBaseAdapter(category, log, name, stopwatch);
                });
            }

            [TestMethod]
            public void NameIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                string name = null;
                var stopwatch = Mock.Of<IStopwatch>();

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    new ActivityBaseAdapter(category, log, name, stopwatch);
                });
            }

            [TestMethod]
            public void StopwatchIsNull_SetsStopwatchToStopwatchAdapter()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                IStopwatch stopwatch = null;

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsInstanceOfType(activity.Stopwatch, typeof(StopwatchAdapter));
            }

            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsInstanceOfType(activity, typeof(ActivityBase));
            }

            [TestMethod]
            public void LogsStartMessage()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                mockLog.Verify(l => l.Debug("Start myCategory myName"));
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
            public void LogsEndMessage()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Act
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Debug("End myCategory myName"));
            }

            [TestMethod]
            public void DisposingMoreThanOnce_OnlyLogsEndMessageOnce()
            {
                // Arrange
                var category = "myCategory";
                var mockLog = new Mock<ILog>();
                var log = mockLog.Object;
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();
                var activity = new ActivityBaseAdapter(category, log, name, stopwatch);

                // Act
                activity.Dispose();
                activity.Dispose();

                // Assert
                mockLog.Verify(l => l.Debug("End myCategory myName"), Times.Once);
            }
        }

        private class ActivityBaseAdapter : ActivityBase
        {
            public ActivityBaseAdapter(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }
        }
    }
}

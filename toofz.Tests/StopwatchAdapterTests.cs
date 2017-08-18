using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace toofz.Tests
{
    class StopwatchAdapterTests
    {
        [TestClass]
        public class StartNew
        {
            [TestMethod]
            public void ReturnsInstanceAndInstanceIsRunning()
            {
                // Arrange -> Act
                var adapter = StopwatchAdapter.StartNew();

                // Assert
                Assert.IsInstanceOfType(adapter, typeof(StopwatchAdapter));
                Assert.IsTrue(adapter.IsRunning);
            }
        }

        [TestClass]
        public class IsRunning
        {
            [TestMethod]
            public void ReturnsIsRunningFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act -> Assert
                Assert.AreEqual(stopwatch.IsRunning, adapter.IsRunning);
            }
        }

        [TestClass]
        public class Elapsed
        {
            [TestMethod]
            public void ReturnsElapsedFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.AreEqual(stopwatch.Elapsed, adapter.Elapsed);
            }
        }

        [TestClass]
        public class ElapsedMilliseconds
        {
            [TestMethod]
            public void ReturnsElapsedMillisecondsFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.AreEqual(stopwatch.ElapsedMilliseconds, adapter.ElapsedMilliseconds);
            }
        }

        [TestClass]
        public class ElapsedTicks
        {
            [TestMethod]
            public void ReturnsElapsedTicksFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.AreEqual(stopwatch.ElapsedTicks, adapter.ElapsedTicks);
            }
        }

        [TestClass]
        public class Reset
        {
            [TestMethod]
            public void StopsMeasuringAndResetsElapsedTimeToZero()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Reset();

                // Assert
                Assert.IsFalse(adapter.IsRunning);
                Assert.AreEqual(0, adapter.ElapsedTicks);
            }
        }

        [TestClass]
        public class Restart
        {
            [TestMethod]
            public void ResetsElapsedTimeAndStartsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Restart();

                // Assert
                Assert.IsTrue(adapter.IsRunning);
            }
        }

        [TestClass]
        public class Start
        {
            [TestMethod]
            public void StartsMeasuring()
            {
                // Arrange
                var stopwatch = new Stopwatch();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act
                adapter.Start();

                // Assert
                Assert.IsTrue(adapter.IsRunning);
            }
        }

        [TestClass]
        public class Stop
        {
            [TestMethod]
            public void StopsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Stop();

                // Assert
                Assert.IsFalse(adapter.IsRunning);
            }
        }
    }
}

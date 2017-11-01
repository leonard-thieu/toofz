using System.Diagnostics;
using Xunit;

namespace toofz.Tests
{
    public class StopwatchAdapterTests
    {
        public class StartNew
        {
            [Fact]
            public void ReturnsInstanceAndInstanceIsRunning()
            {
                // Arrange -> Act
                var adapter = StopwatchAdapter.StartNew();

                // Assert
                Assert.IsAssignableFrom<StopwatchAdapter>(adapter);
                Assert.True(adapter.IsRunning);
            }
        }

        public class IsRunning
        {
            [Fact]
            public void ReturnsIsRunningFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act -> Assert
                Assert.Equal(stopwatch.IsRunning, adapter.IsRunning);
            }
        }

        public class Elapsed
        {
            [Fact]
            public void ReturnsElapsedFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.Elapsed, adapter.Elapsed);
            }
        }

        public class ElapsedMilliseconds
        {
            [Fact]
            public void ReturnsElapsedMillisecondsFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.ElapsedMilliseconds, adapter.ElapsedMilliseconds);
            }
        }

        public class ElapsedTicks
        {
            [Fact]
            public void ReturnsElapsedTicksFromStopwatch()
            {
                // Arrange
                var stopwatch = Stopwatch.StartNew();
                var adapter = new StopwatchAdapter(stopwatch);
                adapter.Stop();

                // Act -> Assert
                Assert.Equal(stopwatch.ElapsedTicks, adapter.ElapsedTicks);
            }
        }

        public class Reset
        {
            [Fact]
            public void StopsMeasuringAndResetsElapsedTimeToZero()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Reset();

                // Assert
                Assert.False(adapter.IsRunning);
                Assert.Equal(0, adapter.ElapsedTicks);
            }
        }

        public class Restart
        {
            [Fact]
            public void ResetsElapsedTimeAndStartsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Restart();

                // Assert
                Assert.True(adapter.IsRunning);
            }
        }

        public class Start
        {
            [Fact]
            public void StartsMeasuring()
            {
                // Arrange
                var stopwatch = new Stopwatch();
                var adapter = new StopwatchAdapter(stopwatch);

                // Act
                adapter.Start();

                // Assert
                Assert.True(adapter.IsRunning);
            }
        }

        public class Stop
        {
            [Fact]
            public void StopsMeasuring()
            {
                // Arrange
                var adapter = StopwatchAdapter.StartNew();

                // Act
                adapter.Stop();

                // Assert
                Assert.False(adapter.IsRunning);
            }
        }
    }
}

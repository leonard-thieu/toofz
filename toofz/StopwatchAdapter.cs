using System;
using System.Diagnostics;

namespace toofz
{
    sealed class StopwatchAdapter : IStopwatch
    {
        /// <summary>
        /// Initializes a new <see cref="StopwatchAdapter"/> instance, sets the elapsed time property to zero, and 
        /// starts measuring elapsed time.
        /// </summary>
        /// <returns>
        /// A <see cref="StopwatchAdapter"/> that has just begun measuring elapsed time.
        /// </returns>
        public static StopwatchAdapter StartNew()
        {
            return new StopwatchAdapter(Stopwatch.StartNew());
        }

        internal StopwatchAdapter(Stopwatch stopwatch)
        {
            this.stopwatch = stopwatch;
        }

        readonly Stopwatch stopwatch;

        /// <summary>
        /// Gets a value indicating whether the stopwatch timer is running.
        /// </summary>
        public bool IsRunning => stopwatch.IsRunning;

        /// <summary>
        /// Gets the total elapsed time measured by the current instance.
        /// </summary>
        public TimeSpan Elapsed => stopwatch.Elapsed;

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        public long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in timer ticks.
        /// </summary>
        public long ElapsedTicks => stopwatch.ElapsedTicks;

        /// <summary>
        /// Stops time interval measurement and resets the elapsed time to zero.
        /// </summary>
        public void Reset() => stopwatch.Reset();

        /// <summary>
        /// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
        /// </summary>
        public void Restart() => stopwatch.Restart();

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        public void Start() => stopwatch.Start();

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        public void Stop() => stopwatch.Stop();
    }
}

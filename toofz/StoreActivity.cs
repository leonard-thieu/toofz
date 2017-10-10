using System.Threading;
using log4net;

namespace toofz
{
    public sealed class StoreActivity : ProgressActivityBase<long>
    {
        public StoreActivity(ILog log, string name) : this(log, name, null) { }

        internal StoreActivity(ILog log, string name, IStopwatch stopwatch) : base("Store", log, name, stopwatch) { }

        long rowsAffected;

        public long RowsAffected => rowsAffected;

        public override void Report(long value)
        {
            Interlocked.Add(ref rowsAffected, value);
        }

        #region IDisposable Implementation

        bool disposed;

        public override void Dispose()
        {
            if (disposed)
                return;

            var rows = RowsAffected;
            var total = Stopwatch.Elapsed.TotalSeconds;
            var rate = (rows / total).ToString("F0");

            Log.Info($"{Category} {Name} complete -- {rows} rows affected over {total.ToString("F1")} seconds ({rate} rows per second).");

            disposed = true;
            base.Dispose();
        }

        #endregion
    }
}

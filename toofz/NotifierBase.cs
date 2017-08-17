using System;
using log4net;

namespace toofz
{
    public abstract class NotifierBase : IDisposable
    {
        protected NotifierBase(string category, ILog log, string name, IStopwatch stopwatch = null)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category), $"{nameof(category)} is null.");
            Log = log ?? throw new ArgumentNullException(nameof(log), $"{nameof(log)} is null.");
            Name = name ?? throw new ArgumentNullException(nameof(name), $"{nameof(name)} is null.");
            Stopwatch = stopwatch ?? StopwatchAdapter.StartNew();

            Log.Debug($"Start {Category} {Name}");
        }

        protected string Category { get; }
        protected ILog Log { get; }
        protected string Name { get; }

        public IStopwatch Stopwatch { get; }

        #region IDisposable Members

        bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Log.Debug($"End {Category} {Name}");
            }

            disposed = true;
        }

        #endregion
    }
}

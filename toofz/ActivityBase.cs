using System;
using log4net;

namespace toofz
{
    public abstract class ActivityBase : IDisposable
    {
        internal ActivityBase(string category, ILog log, string name, IStopwatch stopwatch)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Stopwatch = stopwatch ?? StopwatchAdapter.StartNew();

            Log.Debug($"Start {Category} {Name}");
        }

        protected string Category { get; }
        protected ILog Log { get; }
        protected string Name { get; }

        internal IStopwatch Stopwatch { get; }

        #region IDisposable Members

        bool disposed;

        public virtual void Dispose()
        {
            if (disposed)
                return;

            Log.Debug($"End {Category} {Name}");

            disposed = true;
        }

        #endregion
    }
}

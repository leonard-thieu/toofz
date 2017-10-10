using System;
using log4net;

namespace toofz
{
    public abstract class ProgressActivityBase<T> : ActivityBase, IProgress<T>
    {
        internal ProgressActivityBase(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }

        public abstract void Report(T value);
    }
}

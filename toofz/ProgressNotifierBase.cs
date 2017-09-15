using System;
using log4net;

namespace toofz
{
    public abstract class ProgressNotifierBase<T> : NotifierBase, IProgress<T>
    {
        internal ProgressNotifierBase(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }

        public abstract void Report(T value);
    }
}

﻿using System.Threading;
using Humanizer;
using log4net;

namespace toofz
{
    public sealed class DownloadNotifier : ProgressNotifierBase<long>
    {
        public DownloadNotifier(ILog log, string name) : this(log, name, null) { }

        internal DownloadNotifier(ILog log, string name, IStopwatch stopwatch) : base("Download", log, name, stopwatch) { }

        long totalBytes;

        public long TotalBytes => totalBytes;

        public override void Report(long value)
        {
            Interlocked.Add(ref totalBytes, value);
        }

        #region IDisposable Members

        bool disposed;

        public override void Dispose()
        {
            if (disposed)
                return;

            var byteSize = totalBytes.Bytes();
            var elapsed = Stopwatch.Elapsed;
            var byteRate = byteSize.Per(elapsed);

            var size = byteSize.Humanize("#.#");
            var time = elapsed.TotalSeconds.ToString("F1");
            var rate = byteRate.Humanize("#.#").Replace("/", "p");

            Log.Info($"{Category} {Name} complete -- {size} over {time} seconds ({rate}).");

            disposed = true;
            base.Dispose();
        }

        #endregion
    }
}

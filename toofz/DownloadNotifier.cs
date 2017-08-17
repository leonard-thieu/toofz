using System;
using System.Threading;
using Humanizer;
using log4net;

namespace toofz
{
    public sealed class DownloadNotifier : NotifierBase
    {
        public DownloadNotifier(ILog log, string name, IStopwatch stopwatch = null) :
            base("Download", log, name, stopwatch)
        {
            Progress = new ActionProgress<long>(r => Interlocked.Add(ref totalBytes, r));
        }

        long totalBytes;

        public IProgress<long> Progress { get; }
        public long TotalBytes => totalBytes;

        #region IDisposable Members

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                var byteSize = totalBytes.Bytes();
                var elapsed = Stopwatch.Elapsed;
                var byteRate = byteSize.Per(elapsed);

                var size = byteSize.Humanize("#.#");
                var time = elapsed.TotalSeconds.ToString("F1");
                var rate = byteRate.Humanize("#.#").Replace("/", "p");

                Log.Info($"{Category} {Name} complete -- {size} over {time} seconds ({rate}).");
            }

            disposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}

using System.IO;

namespace toofz.Tests
{
    sealed class UndisposableMemoryStream : MemoryStream
    {
        protected override void Dispose(bool disposing)
        {
            // Do nothing
        }
    }
}

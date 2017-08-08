using System;

namespace toofz.Tests
{
    sealed class UnthrownException : Exception
    {
        public UnthrownException(string stackTrace)
        {
            this.stackTrace = stackTrace;
        }

        readonly string stackTrace;
        public override string StackTrace => stackTrace;
    }
}

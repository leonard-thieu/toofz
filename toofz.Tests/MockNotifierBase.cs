using log4net;

namespace toofz.Tests
{
    sealed class MockNotifierBase : NotifierBase
    {
        public MockNotifierBase(string category, ILog log, string name, IStopwatch stopwatch = null) :
            base(category, log, name, stopwatch)
        {

        }
    }
}

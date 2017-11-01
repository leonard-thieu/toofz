using System;
using log4net;
using Moq;
using Xunit;

namespace toofz.Tests
{
    public class ProgressActivityBaseTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var activity = new ProgressActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsAssignableFrom<ProgressActivityBase<int>>(activity);
            }
        }

        private class ProgressActivityBaseAdapter : ProgressActivityBase<int>
        {
            internal ProgressActivityBaseAdapter(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }

            public override void Report(int value) => throw new NotImplementedException();
        }
    }
}

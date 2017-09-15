using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class ProgressNotifierBaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var category = "myCategory";
                var log = Mock.Of<ILog>();
                var name = "myName";
                var stopwatch = Mock.Of<IStopwatch>();

                // Act
                var notifier = new ProgressNotifierBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsInstanceOfType(notifier, typeof(ProgressNotifierBase<int>));
            }
        }

        class ProgressNotifierBaseAdapter : ProgressNotifierBase<int>
        {
            internal ProgressNotifierBaseAdapter(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }

            public override void Report(int value) => throw new NotImplementedException();
        }
    }
}

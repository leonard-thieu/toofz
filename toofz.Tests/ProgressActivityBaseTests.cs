using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    public class ProgressActivityBaseTests
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
                var activity = new ProgressActivityBaseAdapter(category, log, name, stopwatch);

                // Assert
                Assert.IsInstanceOfType(activity, typeof(ProgressActivityBase<int>));
            }
        }

        private class ProgressActivityBaseAdapter : ProgressActivityBase<int>
        {
            internal ProgressActivityBaseAdapter(string category, ILog log, string name, IStopwatch stopwatch) : base(category, log, name, stopwatch) { }

            public override void Report(int value) => throw new NotImplementedException();
        }
    }
}

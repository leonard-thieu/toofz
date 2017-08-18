using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.TestsShared;

namespace toofz.Tests
{
    public class TextWriterExtensionsTests
    {
        [TestClass]
        public class WriteLineStart
        {
            [TestMethod]
            public void WritesLineThenValue()
            {
                // Arrange
                var mockWriter = new Mock<TextWriter>();

                // Act
                TextWriterExtensions.WriteLineStart(mockWriter.Object, null);

                // Assert
                mockWriter.Verify(x => x.WriteLine(), Times.Once);
                mockWriter.Verify(x => x.Write((object)null), Times.Once);
            }

            [TestMethod]
            public void WriterIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                TextWriter writer = null;

                // Act
                var ex = Record.Exception(() => TextWriterExtensions.WriteLineStart(writer, null));

                // Assert
                Assert.IsNotNull(ex);
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }
    }
}

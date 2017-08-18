using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.Tests
{
    class TextWriterExtensionsTests
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

                // Act -> Assert
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    TextWriterExtensions.WriteLineStart(writer, null);
                });
            }
        }
    }
}

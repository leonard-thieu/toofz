using System.CodeDom.Compiler;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.Tests
{
    class ExceptionRendererTests
    {
        [TestClass]
        public class RenderStackTrace
        {
            [TestMethod]
            public void StackTraceIsNull_DoesNotThrowNullReferenceException()
            {
                // Arrange
                string stackTrace = null;
                using (var sw = new StringWriter())
                using (var indentedTextWriter = new IndentedTextWriter(sw))
                {
                    // Act
                    var ex = Record.Exception(() =>
                    {
                        ExceptionRenderer.RenderStackTrace(stackTrace, indentedTextWriter);
                    });

                    // Assert
                    Assert.IsNull(ex);
                }
            }

            [TestMethod]
            public void StackTraceFromThrownException_RendersStackTraceCorrectly()
            {
                // Arrange
                var ex = ExceptionHelper.GetThrownException();
                using (var sw = new StringWriter())
                using (var indentedTextWriter = new IndentedTextWriter(sw))
                {
                    // Act
                    ExceptionRenderer.RenderStackTrace(ex.StackTrace, indentedTextWriter, true);
                    var output = sw.ToString();

                    // Assert
                    var expected = @"
StackTrace:
    toofz.Tests.ExceptionHelper.ThrowException()
    toofz.TestsShared.Record.Exception(Action testCode)";
                    AssertHelper.NormalizedAreEqual(expected, output);
                }
            }

            [TestMethod]
            public void StackTraceFromUnthrownException_RendersStackTraceCorrectly()
            {
                // Arrange
                var stackTraceStr = @"   at toofz.Tests.ExceptionHelper.ThrowException() in S:\Projects\toofz\toofz.Tests\ExceptionHelper.cs:line 10
   at toofz.TestsShared.Record.Exception(Action testCode) in C:\projects\toofz-testsshared\toofz.TestsShared\Record.cs:line 33";
                var ex = new UnthrownException(stackTraceStr);
                using (var sw = new StringWriter())
                using (var indentedTextWriter = new IndentedTextWriter(sw))
                {
                    // Act
                    ExceptionRenderer.RenderStackTrace(ex.StackTrace, indentedTextWriter, true);
                    var output = sw.ToString();

                    // Assert
                    var expected = @"
StackTrace:
    toofz.Tests.ExceptionHelper.ThrowException()
    toofz.TestsShared.Record.Exception(Action testCode)";
                    AssertHelper.NormalizedAreEqual(expected, output);
                }
            }
        }
    }
}

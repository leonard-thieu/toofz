using System.CodeDom.Compiler;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using toofz.TestsShared;

namespace toofz.Tests
{
    class ExceptionRendererTests
    {
        [TestClass]
        public class RenderObject
        {
            [TestMethod]
            public void RendersException()
            {
                // Arrange
                var ex = ExceptionHelper.GetThrownException();
                var renderer = new ExceptionRenderer();
                using (var sr = new StringWriter())
                {
                    // Act
                    renderer.RenderObject(null, ex, sr, true);
                    var output = sr.ToString();

                    // Assert
                    var expected = @"System.Exception was unhandled
  HResult=-2146233088
  Message=Thrown test exception
  Source=toofz.Tests
  StackTrace:
    toofz.Tests.ExceptionHelper.ThrowException()
    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException[T](Action action, String message, Object[] parameters)";
                    AssertHelper.NormalizedAreEqual(expected, output);
                }
            }
        }

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
                    // Act -> Assert
                    ExceptionRenderer.RenderStackTrace(stackTrace, indentedTextWriter);
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
    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException[T](Action action, String message, Object[] parameters)";
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

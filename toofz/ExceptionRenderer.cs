using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using log4net;
using log4net.ObjectRenderer;

namespace toofz
{
    /// <summary>
    /// Custom log4net renderer for <see cref="Exception"/>.
    /// </summary>
    public sealed class ExceptionRenderer : IObjectRenderer
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(ExceptionRenderer));

        internal static void RenderStackTrace(string stackTrace, IndentedTextWriter indentedWriter)
        {
            var stackFrames = stackTrace.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (stackFrames.Length == 0)
                return;

            indentedWriter.WriteLineStart("StackTrace:");
            indentedWriter.Indent++;

            foreach (var stackFrame in stackFrames)
            {
                if (stackFrame.StartsWith("   at "))
                {
                    // Stack frames from System.Runtime.CompilerServices are generally internals for handling async methods. 
                    // They produce a lot of noise in logs so we filter them out when rendering stack traces.
                    if (!stackFrame.StartsWith("   at System.Runtime.CompilerServices"))
                    {
                        indentedWriter.WriteLineStart(stackFrame);
                    }
                }
                else if (stackFrame.StartsWith("---"))
                {
                    continue;
                }
                else
                {
                    Log.Warn($"Unexpected line while rendering stack trace: '{stackFrame}'.");
                }
            }

            indentedWriter.Indent--;
        }

        /// <summary>
        /// Renders an object of type <see cref="Exception"/> similar to how the Visual Studio Exception Assistant 
        /// renders it.
        /// </summary>
        /// <param name="rendererMap">Not used.</param>
        /// <param name="obj">The exception.</param>
        /// <param name="writer">The writer.</param>
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            var ex = (Exception)obj;
            var type = ex.GetType();

            var indentedWriter = writer as IndentedTextWriter;
            if (indentedWriter == null)
            {
                indentedWriter = new IndentedTextWriter(writer, "  ");

                indentedWriter.Write($"{type} was unhandled");
                indentedWriter.Indent++;
            }

            var properties = type.GetProperties().OrderBy(x => x.Name);
            foreach (var property in properties)
            {
                var name = property.Name;

                switch (name)
                {
                    // Ignored properties
                    case "Data":
                    case "TargetSite":

                    // Special case properties
                    case "StackTrace":
                    case "InnerException":
                        break;

                    default:
                        var value = property.GetValue(ex)?.ToString();
                        if (value != null)
                        {
                            indentedWriter.WriteLineStart($"{name}={value}");
                        }
                        break;
                }
            }

            RenderStackTrace(ex.StackTrace, indentedWriter);

            var innerException = ex.InnerException;
            if (innerException != null)
            {
                type = innerException.GetType();
                indentedWriter.WriteLineStart($"InnerException: {type}");
                indentedWriter.Indent++;
                RenderObject(rendererMap, innerException, indentedWriter);
            }
        }
    }
}

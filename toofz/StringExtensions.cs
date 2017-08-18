using System;
using System.IO;

namespace toofz
{
    /// <summary>
    /// Contains extension methods for <see cref="string" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a <see cref="Stream"/> with <paramref name="value"/> written to it.
        /// </summary>
        /// <param name="value">The value to write to the stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static Stream ToStream(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), $"{nameof(value)} is null.");

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(value);
            sw.Flush();
            ms.Position = 0;

            return ms;
        }
    }
}

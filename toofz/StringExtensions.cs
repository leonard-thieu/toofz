using System;
using System.IO;

namespace toofz
{
    /// <summary>
    /// Contains extension methods for <see cref="string" />.
    /// </summary>
    public static class StringExtensions
    {
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

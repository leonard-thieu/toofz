using System;
using toofz.TestsShared;

namespace toofz.Tests
{
    static class ExceptionHelper
    {
        static void ThrowException()
        {
            throw new Exception("Thrown test exception");
        }

        public static Exception GetThrownException()
        {
            return Record.Exception(ThrowException);
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            return Assert.ThrowsException<Exception>((Action)ThrowException);
        }
    }
}

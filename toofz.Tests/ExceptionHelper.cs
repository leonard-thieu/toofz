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
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                return ex;
            }

            // Unreachable
            return null;
        }

        static void ThrowExceptionWithInnerException()
        {
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                throw new Exception("Thrown test exception with inner exception", ex);
            }
        }

        public static Exception GetThrownExceptionWithInnerException()
        {
            try
            {
                ThrowExceptionWithInnerException();
            }
            catch (Exception ex)
            {
                return ex;
            }

            // Unreachable
            return null;
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace toofz
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    public static class Util
    {
        static IEnvironment environment = new EnvironmentAdapter();

        [ExcludeFromCodeCoverage]
        public static string GetEnvVar(string variable)
        {
            return GetEnvVar(variable, environment);
        }

        internal static string GetEnvVar(string variable, IEnvironment environment)
        {
            return environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) ??
                throw new ArgumentNullException(null, $"The environment variable '{variable}' must be set.");
        }
    }
}

using System;

namespace toofz
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    public static class Util
    {
        public static string GetEnvVar(string variable)
        {
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Machine) ??
                throw new ArgumentNullException(null,
                    $"The environment variable '{variable}' must be set.");
        }
    }
}

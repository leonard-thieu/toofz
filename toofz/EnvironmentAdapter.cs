using System;
using System.Diagnostics.CodeAnalysis;

namespace toofz
{
    [ExcludeFromCodeCoverage]
    public sealed class EnvironmentAdapter : IEnvironment
    {
        /// <summary>
        /// Gets a value indicating whether the current process is running in user interactive mode.
        /// </summary>
        public bool UserInteractive => Environment.UserInteractive;

        /// <summary>
        /// Retrieves the value of an environment variable from the current process or from
        /// the Windows operating system registry key for the current user or local machine.
        /// </summary>
        /// <param name="variable">The name of an environment variable.</param>
        /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values.</param>
        /// <returns>
        /// The value of the environment variable specified by the variable and target parameters,
        /// or null if the environment variable is not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="variable"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="target"/> is not a valid <see cref="EnvironmentVariableTarget"/> value.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission to perform this operation.
        /// </exception>
        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariable(variable, target);
        }
    }
}

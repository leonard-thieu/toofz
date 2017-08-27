using System;

namespace toofz
{
    public interface IEnvironment
    {
        /// <summary>
        /// Gets or sets the fully qualified path of the current working directory.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Attempted to set to an empty string ("").
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Attempted to set to null.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// An I/O error occurred.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// Attempted to set a local path that cannot be found.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the appropriate permission.
        /// </exception>
        string CurrentDirectory { get; set; }
        /// <summary>
        /// Gets a value indicating whether the current process is running in user interactive mode.
        /// </summary>
        bool UserInteractive { get; }

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
        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);
    }
}
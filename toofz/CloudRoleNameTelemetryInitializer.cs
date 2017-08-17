using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace toofz
{
    /// <summary>
    /// A telemetry initializer that sets cloud_RoleName on telemetry.
    /// </summary>
    public sealed class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Gets or sets the cloud role name.
        /// </summary>
        public string CloudRoleName { get; set; }

        /// <summary>
        /// Initializes <see cref="ITelemetry"/> device context.
        /// </summary>
        /// <param name="telemetry">The telemetry to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = CloudRoleName;
        }
    }
}

using System;

namespace Uptrace.OpenTelemetry
{
    /// <summary>
    /// Uptrace configuration options
    /// </summary>
    public class UptraceOptions
    {
        public UptraceOptions()
        {
            var dsn = Environment.GetEnvironmentVariable("UPTRACE_DSN");
            if (!string.IsNullOrEmpty(dsn))
            {
                this.Dsn = dsn;
            }
        }

        public UptraceOptions(string dsn)
        {
            this.Dsn = dsn;
        }

        /// <summary>
        /// Dsn used to send telemetry data to Uptrace
        /// <para/>
        /// <b>Required</b>
        /// </summary>
        public string Dsn { get; set; }
    }
}

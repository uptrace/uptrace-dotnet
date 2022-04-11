using System;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Uptrace.OpenTelemetry
{
    /// <summary>
    /// Extension methods for <see cref="MeterProviderBuilder"/>
    /// </summary>
    public static class MeterProviderBuilderExtensions
    {
        /// <summary>
        /// Configures the <see cref="TracerProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static MeterProviderBuilder AddUptrace(this MeterProviderBuilder builder)
        {
            var opts = new UptraceOptions();
            return builder.AddUptrace(opts);
        }
        
        /// <summary>
        /// Configures the <see cref="MeterProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static MeterProviderBuilder AddUptrace(
            this MeterProviderBuilder builder,
            string dsn
        )
        {
            var opts = new UptraceOptions(dsn);
            return builder.AddUptrace(opts);
        }

        /// <summary>
        /// Configures the <see cref="MeterProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static MeterProviderBuilder AddUptrace(
            this MeterProviderBuilder builder,
            UptraceOptions opts
        )
        {
            if (string.IsNullOrWhiteSpace(opts.Dsn))
                throw new ArgumentException("Uptrace DSN cannot be empty");

            builder
                .AddOtlpExporter(
                    opt =>
                    {
                        opt.Endpoint = opts.OtlpEndpoint;
                        opt.Headers = string.Format("uptrace-dsn={0}", opts.Dsn);
                    }
                );

            return builder;
        }
    }
}
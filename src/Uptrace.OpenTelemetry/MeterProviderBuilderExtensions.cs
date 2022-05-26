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
        public static MeterProviderBuilder AddUptrace(this MeterProviderBuilder builder, string dsn)
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
                throw new ArgumentException(
                    "Uptrace DSN cannot be empty (set UPTRACE_DSN env var)"
                );

            builder.AddOtlpExporter(
                (exporterOptions, metricReaderOptions) =>
                {
                    exporterOptions.Endpoint = opts.OtlpEndpoint;
                    exporterOptions.Headers = string.Format("uptrace-dsn={0}", opts.Dsn);
                    metricReaderOptions.TemporalityPreference =
                        MetricReaderTemporalityPreference.Delta;
                }
            );

            return builder;
        }
    }
}

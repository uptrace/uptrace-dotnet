using System;
using OpenTelemetry.Trace;

namespace Uptrace.OpenTelemetry
{
    /// <summary>
    /// Extension methods for <see cref="TracerProviderBuilder"/>
    /// </summary>
    public static class TracerProviderBuilderExtensions
    {
        /// <summary>
        /// Configures the <see cref="TracerProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static TracerProviderBuilder AddUptrace(this TracerProviderBuilder builder)
        {
            var opts = new UptraceOptions();
            return builder.AddUptrace(opts);
        }

        /// <summary>
        /// Configures the <see cref="TracerProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static TracerProviderBuilder AddUptrace(
            this TracerProviderBuilder builder,
            string dsn
        )
        {
            var opts = new UptraceOptions(dsn);
            return builder.AddUptrace(opts);
        }

        /// <summary>
        /// Configures the <see cref="TracerProviderBuilder"/> to send telemetry data to Uptrace
        /// </summary>
        public static TracerProviderBuilder AddUptrace(
            this TracerProviderBuilder builder,
            UptraceOptions opts
        )
        {
            if (string.IsNullOrWhiteSpace(opts.Dsn))
                throw new ArgumentException(
                    "Uptrace DSN cannot be empty (set UPTRACE_DSN env var)"
                );

            builder
                .AddOtlpExporter(
                    opt =>
                    {
                        opt.Endpoint = opts.OtlpEndpoint;
                        opt.Headers = string.Format("uptrace-dsn={0}", opts.Dsn);
                    }
                )
                .AddProcessor(new BaggageSpanProcessor())
                .AddXRayTraceId();

            return builder;
        }
    }
}

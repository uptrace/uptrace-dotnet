using System;

using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Exporter;

namespace Uptrace.OpenTelemetry
{
    /// <summary>
    /// Extension methods for <see cref="OpenTelemetryLoggerOptions"/>
    /// </summary>
    public static class OpenTelemetryLoggerOptionsExtensions
    {
        /// <summary>
        /// Configures the <see cref="OpenTelemetryLoggerOptions"/> to send telemetry data to Uptrace
        /// </summary>
        public static OpenTelemetryLoggerOptions AddUptrace(
            this OpenTelemetryLoggerOptions loggerOptions
        )
        {
            var opts = new UptraceOptions();
            return loggerOptions.AddUptrace(opts);
        }

        /// <summary>
        /// Configures the <see cref="OpenTelemetryLoggerOptions"/> to send telemetry data to Uptrace
        /// </summary>
        public static OpenTelemetryLoggerOptions AddUptrace(
            this OpenTelemetryLoggerOptions loggerOptions,
            string dsn
        )
        {
            var opts = new UptraceOptions(dsn);
            return loggerOptions.AddUptrace(opts);
        }

        /// <summary>
        /// Configures the <see cref="OpenTelemetryLoggerOptions"/> to send telemetry data to Uptrace
        /// </summary>
        public static OpenTelemetryLoggerOptions AddUptrace(
            this OpenTelemetryLoggerOptions loggerOptions,
            UptraceOptions opts
        )
        {
            if (string.IsNullOrWhiteSpace(opts.Dsn))
                throw new ArgumentException(
                    "Uptrace DSN cannot be empty (set UPTRACE_DSN env var)"
                );

            loggerOptions.AddOtlpExporter((exporterOptions, processorOptions) =>
            {
                exporterOptions.Protocol = OtlpExportProtocol.Grpc;
                exporterOptions.Endpoint = opts.OtlpGrpcEndpoint;
                exporterOptions.Headers = string.Format("uptrace-dsn={0}", opts.Dsn);

                processorOptions.ExportProcessorType = ExportProcessorType.Batch;
                processorOptions.BatchExportProcessorOptions = new BatchExportLogRecordProcessorOptions();
            });

            return loggerOptions;
        }
    }
}

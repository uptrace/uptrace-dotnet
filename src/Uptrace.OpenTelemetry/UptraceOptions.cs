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
                parseDsn(dsn);
            }
        }

        public UptraceOptions(string dsn)
        {
            parseDsn(dsn);
        }

        private void parseDsn(string dsn)
        {
            this.Dsn = dsn;

            var uri = new Uri(dsn);
            if (uri.Host == "uptrace.dev" || uri.Host == "api.uptrace.dev")
            {
                this.OtlpGrpcEndpoint = new Uri("https://otlp.uptrace.dev:4317");
            }
            else
            {

                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                if (!Int32.TryParse(query["grpc"], out var grpcPort))
                {
                    grpcPort = 14317;
                }

                this.OtlpGrpcEndpoint =
                    new UriBuilder
                    {
                        Scheme = uri.Scheme,
                        Host = uri.DnsSafeHost,
                        Port = grpcPort,
                    }.Uri;
            }
        }

        /// <summary>
        /// Dsn used to send telemetry data to Uptrace
        /// <para/>
        /// <b>Required</b>
        /// </summary>
        public string Dsn { get; set; }

        /// <summary>
        /// OTLP endpoint for gRPC.
        /// </summary>
        public Uri OtlpGrpcEndpoint { get; set; }
    }
}

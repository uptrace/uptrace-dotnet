using System;
using System.Diagnostics; //
using System.Collections.Generic; //

using Microsoft.Extensions.Logging;

using OpenTelemetry; //
using OpenTelemetry.Trace; //
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Uptrace.OpenTelemetry; //

namespace Example.Logs
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceName = "myservice";
            var serviceVersion = "1.0.0";

            // Firstly, let's configure tracing.
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("*") // subscribe to all activity sources
                .SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddEnvironmentVariableDetector()
                        .AddTelemetrySdk()
                        .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                )
                // copy your project DSN here or use UPTRACE_DSN env var
                //.AddUptrace("https://<token>@uptrace.dev/<project_id>")
                .AddUptrace()
                .Build();
            using var source = new ActivitySource("app_or_lib_name");

            // Secondly, let's configure logging.
            using var loggerFactory = LoggerFactory.Create(
                builder =>
                {
                    builder.AddOpenTelemetry(
                        options =>
                        {
                            options.IncludeScopes = true;
                            options.ParseStateValues = true;
                            options.IncludeFormattedMessage = true;
                            options
                                .SetResourceBuilder(
                                    ResourceBuilder
                                        .CreateDefault()
                                        .AddEnvironmentVariableDetector()
                                        .AddTelemetrySdk()
                                        .AddService(
                                            serviceName: serviceName,
                                            serviceVersion: serviceVersion
                                        )
                                )
                                .AddConsoleExporter()
                                .AddUptrace();
                        }
                    );
                }
            );

            // Lastly, create a trace with some logs.
            using (var main = source.StartActivity("main-operation"))
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogInformation("Hello from {name} {price}.", "tomato", 2.99);
                logger.LogError("This is an error");

                Console.WriteLine(
                    string.Format(
                        "https://app.uptrace.dev/traces/{0}",
                        Activity.Current.Context.TraceId
                    )
                );
            }
        }
    }
}

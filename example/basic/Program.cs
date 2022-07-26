using System;
using System.Diagnostics;
using System.Collections.Generic;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Uptrace.OpenTelemetry;

namespace Example.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceName = "myservice";
            var serviceVersion = "1.0.0";

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("*") // subscribe to all activity sources
                .SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddEnvironmentVariableDetector()
                        .AddTelemetrySdk()
                        .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                )
                // use UPTRACE_DSN env var
                .AddUptrace() // use UPTRACE_DSN env var
                // or pass DSN explicitly
                //.AddUptrace("https://<token>@uptrace.dev/<project_id>")
                .Build();

            using var source = new ActivitySource("app_or_lib_name");

            using (var main = source.StartActivity("main-operation"))
            {
                using (var child1 = source.StartActivity("child1-of-main"))
                {
                    child1?.AddEvent(
                        new ActivityEvent(
                            "log",
                            DateTime.UtcNow,
                            new ActivityTagsCollection(
                                new Dictionary<string, object>
                                {
                                    { "log.severity", "error" },
                                    { "log.message", "User not found" },
                                    { "enduser.id", 123 },
                                }
                            )
                        )
                    );
                }

                using (var child2 = source.StartActivity("child2-of-main"))
                {
                    try
                    {
                        throw new ArgumentException("just an exception");
                    }
                    catch (Exception ex)
                    {
                        child2?.SetStatus(Status.Error.WithDescription(ex.Message));
                        child2?.RecordException(ex);
                    }
                }

                main?.SetTag("http.method", "GET");

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

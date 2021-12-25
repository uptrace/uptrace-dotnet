using System;
using System.Diagnostics;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Uptrace.OpenTelemetry;
using StackExchange.Redis;

namespace Example.Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            using var redis = ConnectionMultiplexer.Connect("localhost:6379");
            var db = redis.GetDatabase();

            using var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource("*") // subscribe to all activity sources
                .SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddEnvironmentVariableDetector()
                        .AddService("myservice")
                )
                .AddRedisInstrumentation(redis)
                // copy your project DSN here or use UPTRACE_DSN env var
                //.AddUptrace("https://<token>@api.uptrace.dev/<project_id>")
                .AddUptrace()
                .Build();

            using var source = new ActivitySource("app_or_lib_name");

            using (var main = source.StartActivity("main-operation"))
            {
                string value = "abcdefg";
                db.StringSet("mykey", value);

                value = db.StringGet("mykey");
                Console.WriteLine(value);

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

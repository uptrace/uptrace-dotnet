﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter;

namespace Example.Metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceName = "myservice";
            var serviceVersion = "1.0.0";

            using var meter = new Meter("TestMeter");

            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddRuntimeInstrumentation()
                .AddMeter("*")
                .SetResourceBuilder(
                    ResourceBuilder
                        .CreateDefault()
                        .AddEnvironmentVariableDetector()
                        .AddTelemetrySdk()
                        .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                )
                .AddOtlpExporter(
                    opt =>
                    {
                        var dsn = Environment.GetEnvironmentVariable("UPTRACE_DSN");
                        opt.Protocol = OtlpExportProtocol.Grpc;
                        opt.Endpoint = new Uri("https://otlp.uptrace.dev:4317");
                        opt.Headers = string.Format("uptrace-dsn={0}", dsn);
                    }
                )
                .Build();

            var counter = meter.CreateCounter<int>("counter", "things", "A count of things");
            var histogram = meter.CreateHistogram<int>("histogram");

            System.Console.WriteLine("Press any key to exit.");
            while (!System.Console.KeyAvailable)
            {
                counter.Add(10);

                counter.Add(100, new KeyValuePair<string, object>("tag1", "value1"));

                counter.Add(
                    200,
                    new KeyValuePair<string, object>("tag1", "value2"),
                    new KeyValuePair<string, object>("tag2", "value2")
                );

                histogram.Record(10);

                histogram.Record(100, new KeyValuePair<string, object>("tag1", "value1"));

                histogram.Record(
                    200,
                    new KeyValuePair<string, object>("tag1", "value2"),
                    new KeyValuePair<string, object>("tag2", "value2")
                );

                Task.Delay(500).Wait();
            }
        }
    }
}

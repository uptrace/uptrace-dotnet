# Uptrace for .NET

![build workflow](https://github.com/uptrace/uptrace-dotnet/actions/workflows/build.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Uptrace.OpenTelemetry)](https://www.nuget.org/packages/Uptrace.OpenTelemetry)
[![Documentation](https://img.shields.io/badge/uptrace-documentation-informational)](https://uptrace.dev/get/opentelemetry-dotnet)
[![Chat](https://img.shields.io/badge/-telegram-red?color=white&logo=telegram&logoColor=black)](https://t.me/uptrace)

<a href="https://uptrace.dev/get/opentelemetry-dotnet">
  <img src="https://uptrace.dev/devicon/dot-net-original.svg" height="200px" />
</a>

## Introduction

uptrace-dotnet is an OpenTelemetry distribution configured to export
[traces](https://uptrace.dev/opentelemetry/distributed-tracing),
[metrics](https://uptrace.dev/opentelemetry/metrics), and
[logs](https://uptrace.dev/opentelemetry/logs) to Uptrace.

## Installation

Install uptrace-dotnet:

```shell
dotnet add package Uptrace.OpenTelemetry
```

## Tracing

You can configure the Uptrace client using a DSN (Data Source Name, e.g.
`https://<secret>@api.uptrace.dev?grpc=4317`) from the project settings page.

```cs
using System;
using System.Diagnostics;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("*") // subscribe to all activity sources
    .SetResourceBuilder(
        ResourceBuilder
            .CreateDefault()
            .AddEnvironmentVariableDetector()
            .AddService("myservice")
    )
    // copy your project DSN here or use UPTRACE_DSN env var
    //.AddUptrace("https://<secret>@api.uptrace.dev?grpc=4317")
    .AddUptrace()
    .Build();
```

See the [basic example](example/basic) to try OpenTelemetry and Uptrace.

## Metrics

```cs
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddMeter("MyMeter") // subscribe to all the meters you want
    .AddMeter("*") // or subscribe to all meters
    .SetResourceBuilder(
        ResourceBuilder
            .CreateDefault()
            .AddEnvironmentVariableDetector()
            .AddService(serviceName: "myservice", serviceVersion: "1.0.0")
    )
    // copy your project DSN here or use UPTRACE_DSN env var
    .AddUptrace()
    .Build();
```

See the [metrics example](example/metrics) to try OpenTelemetry and Uptrace.

## Logs

```cs
using Microsoft.Extensions.Logging;

using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.IncludeScopes = true;
        options.ParseStateValues = true;
        options.IncludeFormattedMessage = true;
        options
            .SetResourceBuilder(
                ResourceBuilder
                    .CreateDefault()
                    .AddEnvironmentVariableDetector()
                    .AddService(serviceName: "myservice", serviceVersion: "1.0.0")
            )
            // copy your project DSN here or use UPTRACE_DSN env var
            .AddUptrace();
    });
});

var logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Hello from {name} {price}.", "tomato", 2.99);
```

See the [logs example](example/logs) to try OpenTelemetry and Uptrace.

## Runtime metrics

To collect telemetry about runtime behavior, install
[OpenTelemetry.Instrumentation.Runtime](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.Runtime):

```shell
dotnet add package OpenTelemetry.Instrumentation.Runtime
```

And enable the instrumentation:

```cs
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddRuntimeInstrumentation()
    .AddUptrace()
    .Build();
```

## Links

- [Examples](example)
- [Documentation](https://uptrace.dev/get/opentelemetry-dotnet)
- [OpenTelemetry .NET guides](https://uptrace.dev/guides)

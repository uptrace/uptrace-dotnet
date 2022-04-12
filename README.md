# Uptrace for .NET

[![Documentation](https://img.shields.io/badge/uptrace-documentation-informational)](https://docs.uptrace.dev/guide/dotnet.html)

<a href="https://docs.uptrace.dev/guide/dotnet.html">
  <img src="https://docs.uptrace.dev/devicon/dot-net-original.svg" height="200px" />
</a>

## Introduction

uptrace-dotnet is an OpenTelemery distribution configured to export
[traces](https://opentelemetry.uptrace.dev/guide/distributed-tracing.html) to Uptrace.

## Installation

Install uptrace-dotnet:

```shell
dotnet add package Uptrace.OpenTelemetry
```

## Usage Tracer

You can configure Uptrace client using a DSN (Data Source Name, e.g.
`https://<token>@uptrace.dev/<project_id>`) from the project settings page.

```cs
using System;
using System.Diagnostics;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

var openTelemetry = Sdk.CreateTracerProviderBuilder()
    .AddSource("*") // subscribe to all activity sources
    .SetResourceBuilder(
        ResourceBuilder
            .CreateDefault()
            .AddEnvironmentVariableDetector()
            .AddService("myservice")
    )
    // copy your project DSN here or use UPTRACE_DSN env var
    //.AddUptrace("https://<token>@api.uptrace.dev/<project_id>")
    .AddUptrace()
    .Build();
```

See the [basic example](example/basic) to try OpenTelemetry and Uptrace.

## Usage Meter

You can configure Uptrace client using a DSN (Data Source Name, e.g.
`https://<token>@uptrace.dev/<project_id>`) from the project settings page.

```cs
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddMeter("MyMeter") // subscribe to all the meters you want
    .AddMeter("*") // or subscribe to all meters

    // see https://docs.microsoft.com/en-us/dotnet/core/diagnostics/available-counters for more
    //.AddMeter("System.Runtime")

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

## Runtime metrics

To collect telemetry about runtime behavior, install
[OpenTelemetry.Instrumentation.Runtime](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/src/OpenTelemetry.Instrumentation.Runtime):

```shell
dotnet add package OpenTelemetry.Instrumentation.Runtime
```

And enable the instrumentation:

```cs
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddRuntimeMetrics()
    .Build();
```

## Links

- [Examples](example)
- [Documentation](https://docs.uptrace.dev/guide/dotnet.html)
- [.NET instrumentations](https://opentelemetry.uptrace.dev/instrumentations.html?lang=dotnet)

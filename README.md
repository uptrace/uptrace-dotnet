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
`https://<token>@api.uptrace.dev/<project_id>`) from the project settings page.

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

Run the [basic example](example/basic) to try OpenTelemetry and Uptrace.


## Usage Meter

You can configure Uptrace client using a DSN (Data Source Name, e.g.
`https://<token>@api.uptrace.dev/<project_id>`) from the project settings page.

```cs
using System;
using System.Diagnostics;

using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

using Uptrace.OpenTelemetry;

var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddMeter("MyMeter") // subscribe to all the meters you want
    .SetResourceBuilder(
        ResourceBuilder
            .CreateDefault()
            .AddEnvironmentVariableDetector()
            .AddService("myservice")
    )
    // copy your project DSN here or use UPTRACE_DSN env var
    .AddUptrace()
    .Build();
```


## Links

- [Examples](example)
- [Documentation](https://docs.uptrace.dev/guide/dotnet.html)
- [Instrumentations](https://opentelemetry.uptrace.dev/instrumentations.html?lang=dotnet)

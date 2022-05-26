# Contributing

## Code formatting with csharpier

Install:

```shell
dotnet tool install -g csharpier
```

Use:

```shell
dotnet csharpier .
```

## Code formatting with dotnet-format

Install:

```shell
dotnet tool install -g dotnet-format
```

Use:

```shell
dotnet format
```

## Creating a solution

```shell
dotnet new sln --name uptrace
dotnet new classlib --name Uptrace.OpenTelemetry --output src/Uptrace.OpenTelemetry
dotnet sln add ./src/Uptrace.OpenTelemetry/Uptrace.OpenTelemetry.csproj
```

Adding a new project to the solution:

```shell
dotnet new console --name Example.Otlp --output example/otlp
dotnet sln add ./example/otlp/Example.Otlp.csproj
```

## Adding a package

```shell
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol.Logs
```

## Logging and self-diagnostics

To enable logging and self-diagnostics, see
https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry#troubleshooting

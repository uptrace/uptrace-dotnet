# Contributing

## Building

```shell
dotnet build uptrace.sln
```

Release builds treat warnings as errors:

```shell
dotnet build -c Release uptrace.sln
```

## Code formatting with CSharpier

Install:

```shell
dotnet tool install -g csharpier
```

Use:

```shell
dotnet csharpier format .
```

## Commit messages

Pull request commits are checked with [commitlint](https://commitlint.js.org/) and must follow the
[Conventional Commits](https://www.conventionalcommits.org/) format, e.g. `feat: add logs support`
or `chore: bump dependencies`.

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
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
```

## Logging and self-diagnostics

To enable logging and self-diagnostics, see
https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry#troubleshooting

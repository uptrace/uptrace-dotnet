# AGENTS.md

Guidance for AI coding agents working in this repository.

## What this repo is

`Uptrace.OpenTelemetry` is a small .NET library (an OpenTelemetry "distro") that
configures the OpenTelemetry SDK to export traces, metrics, and logs to
[Uptrace](https://uptrace.dev/) over OTLP/gRPC. The public API is a set of
`AddUptrace()` extension methods plus `UptraceOptions` (DSN parsing).

## Layout

- `src/Uptrace.OpenTelemetry/` — the library (the only shipped project; NuGet
  package `Uptrace.OpenTelemetry`, version set in the `.csproj`)
- `example/` — runnable console examples (`basic`, `logs`, `metrics`,
  `metrics-otlp`, `redis`); these are never packaged
- `build/` — shared MSBuild props; `src/` imports `Common.prod.props`,
  `example/` imports `Common.nonprod.props`, both import `Common.props`
- `uptrace.sln` — solution referencing the library and examples

## Commands

```shell
dotnet build uptrace.sln        # build everything (also packs the library)
dotnet csharpier format .       # format C# (printWidth 100, see .csharpierrc.yml)
make                            # fmt + restore + build
```

There are no tests. Verify changes by building the solution (Release too:
`dotnet build -c Release uptrace.sln` — warnings are errors in Release) and,
when behavior changes, by running an example with a real DSN:
`UPTRACE_DSN=... dotnet run --project example/basic`.

## Conventions

- Conventional Commits are enforced on PRs by commitlint
  (`.github/workflows/commitlint.yml`): `feat:`, `fix:`, `chore:`, etc.
- Code style: csharpier formatting; XML doc comments on all public members.
- Keep the library dependency-light: only the OTLP exporter and the AWS
  X-Ray trace-ID extension. Instrumentation packages belong in examples/docs,
  not in the library.

## Releasing

Maintainer-driven; agents should not bump versions or tag unless asked.
See "Releasing a new version" in [CONTRIBUTING.md](CONTRIBUTING.md) for the
step-by-step flow (version bump, changelog, `vX.Y.Z` tag, NuGet push).

## Gotchas

- `UptraceOptions` special-cases the `uptrace.dev` / `api.uptrace.dev` hosts
  (cloud endpoint `https://otlp.uptrace.dev:4317`); any other DSN host is
  treated as self-hosted with gRPC port from the `?grpc=` query param
  (default 14317). Don't break either path.
- The DSN is sent as the `uptrace-dsn` OTLP header — it is a secret; never log
  it in examples.
- Metrics export uses delta temporality intentionally (Uptrace expects it).

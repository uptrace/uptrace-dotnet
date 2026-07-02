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

## Releasing a new version

1. Bump `<Version>` in `src/Uptrace.OpenTelemetry/Uptrace.OpenTelemetry.csproj`.

2. Update `CHANGELOG.md` using [conventional-changelog](https://github.com/conventional-changelog/conventional-changelog):

   ```shell
   npx conventional-changelog-cli -p angular -i CHANGELOG.md -s
   ```

3. Commit and push the release commit together with a `vX.Y.Z` tag:

   ```shell
   git add -A
   git commit -m "chore: release vX.Y.Z"
   git tag vX.Y.Z
   git push origin master vX.Y.Z
   ```

   Pushing the tag triggers `.github/workflows/release.yml`, which verifies that the tag matches
   `<Version>` in the csproj, packs the library, publishes it to nuget.org, and creates the GitHub
   release. Publishing requires the `NUGET_API_KEY` repository secret (Settings → Secrets and
   variables → Actions) containing a nuget.org API key with push permission for
   `Uptrace.OpenTelemetry`.

4. Check that the new version appears on
   [nuget.org/packages/Uptrace.OpenTelemetry](https://www.nuget.org/packages/Uptrace.OpenTelemetry).

## Logging and self-diagnostics

To enable logging and self-diagnostics, see
https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/src/OpenTelemetry#troubleshooting

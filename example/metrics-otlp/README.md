# Metrics example without using uptrace-dotnet

This example configures OpenTelemetry to export metrics to Uptrace without using uptrace-dotnet
package. If you don't have experience with OpenTelemetry, you should prefer using
[metrics](../metrics) example that relies on uptrace-dotnet package to configure OpenTelemetry for
you.

To run this example:

```shell
UPTRACE_DSN=https://<token>@uptrace.dev/<project_id> dotnet run
```

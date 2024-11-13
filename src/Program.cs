using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("practical-otel-azure-functions"))
            .WithTracing(tracingBuilder =>
            {
                tracingBuilder
                    .SetSampler<AlwaysOnSampler>()
                    .AddSource(DiagnosticConfig.Source.Name)
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter();
            });
    })
    .Build();

host.Run();

public static class DiagnosticConfig
{
    public static readonly ActivitySource Source = new("practical-otel-azure-functions");
}
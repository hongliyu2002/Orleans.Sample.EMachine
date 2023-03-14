using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using OpenTelemetry.Trace;

namespace EMachine.Sales.Orleans.EntityFrameworkCore.Contributors;

internal sealed class TracerProviderContributor : ITracerProviderContributor
{
    /// <inheritdoc />
    public void Configure(TracerProviderBuilder builder, IServiceConfigurationContext context)
    {
        // https://github.com/open-telemetry/opentelemetry-dotnet-contrib
        builder.AddEntityFrameworkCoreInstrumentation();
    }
}

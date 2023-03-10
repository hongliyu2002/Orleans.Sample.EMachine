using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using OpenTelemetry.Trace;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class TracerProviderContributor : ITracerProviderContributor
{
    /// <inheritdoc />
    public void Configure(TracerProviderBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<AdoNetClusteringOptions>();
        switch (options.DbProvider)
        {
            case AdoNetDbProvider.SqlServer:
                builder.AddSqlClientInstrumentation();
                break;
            case AdoNetDbProvider.PostgreSQL:
                break;
            case AdoNetDbProvider.MySQL:
                builder.AddMySqlDataInstrumentation();
                break;
            case AdoNetDbProvider.Oracle:
                break;
        }
    }
}

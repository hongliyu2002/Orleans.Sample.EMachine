using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using Machine.Orleans.Server.Providers.AdoNet;
using OpenTelemetry.Trace;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class TracerProviderContributor : ITracerProviderContributor
{
    /// <inheritdoc />
    public void Configure(TracerProviderBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<AdoNetClusteringOptions>();
        switch (options.DatabaseProvider)
        {
            case AdoNetDatabaseProvider.SqlServer:
                builder.AddSqlClientInstrumentation();
                break;
            case AdoNetDatabaseProvider.PostgreSql:
                break;
            case AdoNetDatabaseProvider.MySql:
                builder.AddMySqlDataInstrumentation();
                break;
            case AdoNetDatabaseProvider.Oracle:
                break;
        }
    }
}

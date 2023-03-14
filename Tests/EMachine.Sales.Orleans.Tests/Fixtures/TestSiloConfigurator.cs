using EMachine.Sales.Orleans.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder silo)
    {
        silo.AddMemoryGrainStorage(Constants.PubSubStoreName)
            .AddMemoryGrainStorage(Constants.SalesStoreName)
            .AddMemoryGrainStorage(Constants.ManagementStoreName)
            .AddMemoryGrainStorage(Constants.BankingStoreName)
            .AddLogStorageBasedLogConsistencyProvider(Constants.LogConsistencyStoreName)
            .AddStreaming()
            .AddMemoryStreams(Constants.StreamProviderName);
        silo.Services.AddDbContextPool<SalesDbContext>(options => options.UseSqlite("Data Source=Sales.db"));
    }
}

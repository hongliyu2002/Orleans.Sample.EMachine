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
    }
}

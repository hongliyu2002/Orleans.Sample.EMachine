using Orleans.TestingHost;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("PubSubStore")
                   .AddMemoryGrainStorage("SalesStore")
                   .AddMemoryGrainStorage("ManagementStore")
                   .AddMemoryGrainStorage("BankingStore")
                   .AddLogStorageBasedLogConsistencyProvider("EventStore")
                   .AddMemoryStreams(Constants.StreamProviderName);
    }
}

using Orleans.TestingHost;

namespace EMachine.Sales.Domain.Tests.Fixtures;

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
                   .AddMemoryStreams("Default");
    }
}

using Orleans.TestingHost;

namespace EMachine.Sales.Domain.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("PubSubStore")
                   .AddMemoryGrainStorage("SnackStore")
                   .AddLogStorageBasedLogConsistencyProvider("SnackEventStore")
                   .AddMemoryStreams("Default");
    }
}

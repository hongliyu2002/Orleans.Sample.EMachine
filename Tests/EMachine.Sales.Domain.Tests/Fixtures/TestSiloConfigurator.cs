using Orleans.TestingHost;

namespace EMachine.Sales.Domain.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("PubSubStore")
                   .AddMemoryGrainStorage("SnackStore")
                   .AddMemoryGrainStorage("SnackMachineStore")
                   .AddLogStorageBasedLogConsistencyProvider("EventStore")
                   .AddMemoryStreams("Default");
    }
}

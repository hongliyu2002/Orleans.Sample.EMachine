using Orleans.TestingHost;

namespace EMachine.Domain.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("PubSubStore")
                   .AddMemoryGrainStorage("MoneyStore")
                   .AddLogStorageBasedLogConsistencyProvider("MoneyEventStore")
                   .AddMemoryGrainStorage("SnackStore")
                   .AddLogStorageBasedLogConsistencyProvider("SnackEventStore")
                   .AddMemoryStreams("Default");
    }
}

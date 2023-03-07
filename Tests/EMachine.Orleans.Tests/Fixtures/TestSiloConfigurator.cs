using Orleans.TestingHost;

namespace EMachine.Orleans.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("PubSubStore").AddMemoryGrainStorage("MoneyStore").AddLogStorageBasedLogConsistencyProvider("MoneyEventStore").AddMemoryStreams("Default");
    }
}

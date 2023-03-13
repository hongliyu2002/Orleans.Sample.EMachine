using Orleans.TestingHost;

namespace EMachine.Orleans.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder silo)
    {
        silo.AddMemoryGrainStorage("PubSubStore").AddMemoryGrainStorage("MoneyStore").AddLogStorageBasedLogConsistencyProvider("MoneyEventStore").AddMemoryStreams("Default");
    }
}

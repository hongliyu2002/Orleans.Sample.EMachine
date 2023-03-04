using Orleans.TestingHost;

namespace EMachine.Domain.Tests.Fixtures;

public class TestSiloConfigurator : ISiloConfigurator
{

    /// <inheritdoc />
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage("MemoryStore")
                   .AddMemoryGrainStorage("PubSubStore")
                   .AddStreaming()
                   .AddMemoryStreams("Default");
    }
}

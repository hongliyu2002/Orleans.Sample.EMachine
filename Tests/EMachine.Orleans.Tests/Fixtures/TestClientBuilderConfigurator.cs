using Microsoft.Extensions.Configuration;
using Orleans.TestingHost;

namespace EMachine.Orleans.Tests.Fixtures;

public class TestClientBuilderConfigurator : IClientBuilderConfigurator
{

    /// <inheritdoc />
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.AddStreaming().AddMemoryStreams("Default");
    }
}

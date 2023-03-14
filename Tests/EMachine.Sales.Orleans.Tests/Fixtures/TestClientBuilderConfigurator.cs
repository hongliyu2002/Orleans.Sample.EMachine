using Fluxera.Extensions.Common;
using Microsoft.Extensions.Configuration;
using Orleans.TestingHost;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

public class TestClientBuilderConfigurator : IClientBuilderConfigurator
{
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.ConfigureServices(services =>
                                        {
                                            services.AddGuidGenerator(options => options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary);
                                        });
        clientBuilder.AddStreaming().AddMemoryStreams(Constants.StreamProviderName);
    }
}

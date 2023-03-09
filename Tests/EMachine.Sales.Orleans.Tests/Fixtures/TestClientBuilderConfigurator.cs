using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Extensions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;

namespace EMachine.Sales.Orleans.Tests.Fixtures;

public class TestClientBuilderConfigurator : IClientBuilderConfigurator
{
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.ConfigureServices(services =>
                                        {
                                            services.AddGuidGenerator(options => options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary);
                                            services.AddDbContextPool<SalesDbContext>(options => options.UseSqlite("Data Source=Sales.db"));
                                        });
        clientBuilder.AddStreaming().AddMemoryStreams(Constants.StreamProviderName);
    }
}

using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisPersistenceOptionsContributor : ConfigureOptionsContributorBase<RedisPersistenceOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Persistence";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisPersistenceOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisPersistenceOptions)", services =>
                                                          {
                                                              services.Configure<RedisPersistenceOptions>(persistence =>
                                                                                                          {
                                                                                                              persistence.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                          });
                                                          });
    }
}

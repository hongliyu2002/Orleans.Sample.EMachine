using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisStorageOptionsContributor : ConfigureOptionsContributorBase<RedisStorageOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Storage";

    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisStorageOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisStorageOptions)", services =>
                                                      {
                                                          services.Configure<RedisStorageOptions>(options =>
                                                                                                  {
                                                                                                      options.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                  });
                                                      });
    }
}

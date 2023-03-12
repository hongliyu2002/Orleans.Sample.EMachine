using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisReminderTableOptionsContributor : ConfigureOptionsContributorBase<RedisReminderTableOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:ReminderTable";

    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisReminderTableOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisReminderTableOptions)", services =>
                                                            {
                                                                services.Configure<RedisReminderTableOptions>(options =>
                                                                                                              {
                                                                                                                  options.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                              });
                                                            });
    }
}

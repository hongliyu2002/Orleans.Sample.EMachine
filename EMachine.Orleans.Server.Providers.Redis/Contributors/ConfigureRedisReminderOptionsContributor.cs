using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisReminderOptionsContributor : ConfigureOptionsContributorBase<RedisReminderOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Reminder";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, RedisReminderOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(RedisReminderOptions)", services =>
                                                       {
                                                           services.Configure<RedisReminderOptions>(reminder =>
                                                                                                    {
                                                                                                        reminder.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                    });
                                                       });
    }
}

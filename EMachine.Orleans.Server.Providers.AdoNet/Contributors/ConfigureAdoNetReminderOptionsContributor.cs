using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Server.Providers.AdoNet.Contributors;

internal sealed class ConfigureAdoNetReminderOptionsContributor : ConfigureOptionsContributorBase<AdoNetReminderOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:AdoNet:Reminder";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, AdoNetReminderOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(AdoNetReminderOptions)", services =>
                                                        {
                                                            services.Configure<AdoNetReminderOptions>(reminder =>
                                                                                                      {
                                                                                                          reminder.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                                                      });
                                                        });
    }
}

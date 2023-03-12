using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Persistence.Contributors;

internal sealed class ConfigurePersistenceOptionsContributor : ConfigureOptionsContributorBase<PersistenceOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Persistence";

    /// <inheritdoc />
    protected override void AdditionalConfigure(IServiceConfigurationContext context, PersistenceOptions createdOptions)
    {
        createdOptions.ConnectionStrings = context.Services.GetOptions<ConnectionStrings>();
        context.Log("Configure(PersistenceOptions)", services =>
                        {
                            services.Configure<PersistenceOptions>(options =>
                                                                   {
                                                                       options.ConnectionStrings = createdOptions.ConnectionStrings;
                                                                   });
                        });
    }
}

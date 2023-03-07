using EMachine.Sales.Domain.Abstractions.Entities;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Persistence;
using JetBrains.Annotations;

namespace EMachine.Sales.EntityFrameworkCore.Contributors;

[UsedImplicitly]
internal sealed class RepositoryContributor : RepositoryContributorBase
{
    public override void ConfigureAggregates(IRepositoryAggregatesBuilder builder, IServiceConfigurationContext context)
    {
        builder.UseFor<SnackBase>();
        builder.UseFor<SnackMachineBase>();
    }
}

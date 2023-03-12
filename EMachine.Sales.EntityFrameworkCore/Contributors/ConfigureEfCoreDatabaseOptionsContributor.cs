using Fluxera.Extensions.Hosting.Modules.Configuration;

namespace EMachine.Sales.EntityFrameworkCore.Contributors;

internal sealed class ConfigureEfCoreDatabaseOptionsContributor : ConfigureOptionsContributorBase<EfCoreDatabaseOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Sales:Database";
}

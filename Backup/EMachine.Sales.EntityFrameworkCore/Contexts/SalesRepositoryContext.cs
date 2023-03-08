using Fluxera.Repository.EntityFrameworkCore;
using JetBrains.Annotations;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

[PublicAPI]
internal sealed class SalesRepositoryContext : EntityFrameworkCoreContext
{

    /// <inheritdoc />
    protected override void ConfigureOptions(EntityFrameworkCoreContextOptions options)
    {
        options.UseDbContext(typeof(SalesDbContext));
    }
}

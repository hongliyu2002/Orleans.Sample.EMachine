using EMachine.Sales.Domain.Entities;
using Fluxera.Repository;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Repositories;

[UsedImplicitly]
internal sealed class SnackBaseRepository : Repository<SnackBase, long>, ISnackBaseRepository
{

    /// <inheritdoc />
    public SnackBaseRepository(IRepository<SnackBase, long> innerRepository)
        : base(innerRepository)
    {
    }
}

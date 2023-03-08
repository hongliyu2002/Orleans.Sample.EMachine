using EMachine.Sales.Domain.Entities;
using Fluxera.Repository;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Repositories;

[UsedImplicitly]
internal sealed class SnackRepository : Repository<Snack, Guid>, ISnackRepository
{
    public SnackRepository(IRepository<Snack, Guid> innerRepository)
        : base(innerRepository)
    {
    }
}

using EMachine.Sales.Domain.Entities;
using Fluxera.Repository;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Repositories;

[UsedImplicitly]
internal sealed class SnackMachineBaseRepository : Repository<SnackMachineBase, Guid>, ISnackMachineBaseRepository
{

    /// <inheritdoc />
    public SnackMachineBaseRepository(IRepository<SnackMachineBase, Guid> innerRepository)
        : base(innerRepository)
    {
    }
}

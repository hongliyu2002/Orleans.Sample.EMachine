using EMachine.Sales.Domain.Entities;
using Fluxera.Repository;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Repositories;

[UsedImplicitly]
internal sealed class SnackMachineRepository : Repository<SnackMachine, Guid>, ISnackMachineRepository
{
    public SnackMachineRepository(IRepository<SnackMachine, Guid> innerRepository)
        : base(innerRepository)
    {
    }
}

using EMachine.Sales.Domain.Entities;
using Fluxera.Repository;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Repositories;

[PublicAPI]
public interface ISnackBaseRepository : IRepository<SnackBase, long>
{
}

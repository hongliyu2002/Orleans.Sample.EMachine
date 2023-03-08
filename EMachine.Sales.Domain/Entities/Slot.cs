using Fluxera.Entity;
using JetBrains.Annotations;

namespace EMachine.Sales.Domain.Entities;

[PublicAPI]
public sealed class Slot : Entity<Slot, Guid>
{
    public Guid MachineUuId { get; set; }

    public int Position { get; set; }

    public SnackPile? SnackPile { get; set; }
}

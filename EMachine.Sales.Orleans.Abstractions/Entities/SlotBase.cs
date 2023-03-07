using Fluxera.Entity;
using JetBrains.Annotations;

namespace EMachine.Sales.Orleans.Abstractions.Entities;

[PublicAPI]
public sealed class SlotBase : Entity<SlotBase, long>
{
    public Guid MachineId { get; set; }

    public int Position { get; set; }

    public long SnackId { get; }

    public int Quantity { get; }

    public decimal Price { get; }
}

using JetBrains.Annotations;

namespace EMachine.Sales.Domain;

[PublicAPI]
public sealed class Slot
{
    public Guid MachineId { get; set; }

    public int Position { get; set; }

    public SnackPile? SnackPile { get; set; }
}

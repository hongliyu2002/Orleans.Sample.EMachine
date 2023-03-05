using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.States;

[GenerateSerializer]
public sealed class Slot
{
    public Slot(Guid snackMachineId, SnackPile snackPile, int position)
    {
        MachineId = Guard.Against.Empty(snackMachineId, nameof(snackMachineId));
        SnackPile = Guard.Against.Null(snackPile, nameof(snackPile));
        Position = Guard.Against.Negative(position, nameof(position));
    }

    [Id(0)]
    public Guid MachineId { get; }

    [Id(1)]
    public SnackPile SnackPile { get; }

    [Id(2)]
    public int Position { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Slot with MachineId:'{MachineId}' SnackPile:'{SnackPile}' Position:{Position}";
    }
}

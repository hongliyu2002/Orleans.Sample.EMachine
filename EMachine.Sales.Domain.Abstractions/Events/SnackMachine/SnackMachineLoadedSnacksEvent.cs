using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadedSnacksEvent : SnackMachineEvent
{
    public SnackMachineLoadedSnacksEvent(Guid id, int position, SnackPile snackPile, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Position = Guard.Against.Negative(position);
        SnackPile = Guard.Against.Null(snackPile);
    }

    [Id(0)]
    public int Position { get; }

    [Id(1)]
    public SnackPile SnackPile { get; }
}

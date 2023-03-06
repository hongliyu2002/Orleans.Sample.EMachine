using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadSnacksCommand : SnackMachineCommand
{
    public SnackMachineLoadSnacksCommand(int position, SnackPile snackPile, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Position = Guard.Against.Negative(position);
        SnackPile = Guard.Against.Null(snackPile);
    }

    [Id(0)]
    public int Position { get; }

    [Id(1)]
    public SnackPile SnackPile { get; }
}

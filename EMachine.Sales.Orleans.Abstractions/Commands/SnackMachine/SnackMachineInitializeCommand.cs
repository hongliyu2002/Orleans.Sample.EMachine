using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializeCommand : SnackMachineCommand
{
    public SnackMachineInitializeCommand(Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Money MoneyInside { get; }

    [Id(1)]
    public IImmutableList<Slot> Slots { get; }
}

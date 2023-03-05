using System.Collections.Immutable;
using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializeCommand : SnackMachineCommand
{
    public SnackMachineInitializeCommand(Money moneyInside, ImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Slots = slots;
        MoneyInside = Guard.Against.Null(moneyInside);
    }

    [Id(0)]
    public Money MoneyInside { get; }

    [Id(1)]
    public ImmutableList<Slot> Slots { get; }
}

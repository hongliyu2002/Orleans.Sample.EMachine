using System.Collections.Immutable;
using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializedEvent : SnackMachineEvent
{
    public SnackMachineInitializedEvent(Guid id, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Money MoneyInside { get; }

    [Id(1)]
    public IImmutableList<Slot> Slots { get; }
}

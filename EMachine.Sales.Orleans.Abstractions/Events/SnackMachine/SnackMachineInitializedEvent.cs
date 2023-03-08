using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializedEvent : SnackMachineEvent
{
    public SnackMachineInitializedEvent(Guid uuId, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Money MoneyInside { get; }

    [Id(1)]
    public IImmutableList<Slot> Slots { get; }
}

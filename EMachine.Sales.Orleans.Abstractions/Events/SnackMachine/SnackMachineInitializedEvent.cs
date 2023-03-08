using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializedEvent : SnackMachineEvent
{
    public SnackMachineInitializedEvent(Guid id, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(id, traceId, operatedAt, operatedBy)
    {
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Money MoneyInside { get; } = Money.Zero;

    [Id(1)]
    public IImmutableList<Slot> Slots { get; }
}

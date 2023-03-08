using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Commands;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInitializeCommand : DomainCommand
{
    public SnackMachineInitializeCommand(Money moneyInside, IImmutableList<Slot> slots, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Money MoneyInside { get; }

    [Id(1)]
    public IImmutableList<Slot> Slots { get; }
}

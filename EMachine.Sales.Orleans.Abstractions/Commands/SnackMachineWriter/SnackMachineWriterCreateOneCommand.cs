using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Commands;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineWriterCreateOneCommand : DomainCommand
{
    public SnackMachineWriterCreateOneCommand(Guid id, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Guid Id { get; } = Guid.Empty;

    [Id(1)]
    public Money MoneyInside { get; }

    [Id(2)]
    public IImmutableList<Slot> Slots { get; }
}

using System.Collections.Immutable;
using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRepositoryCreateOneCommand : SnackMachineRepositoryCommand
{
    public SnackMachineRepositoryCreateOneCommand(Guid id, Money moneyInside, IImmutableList<Slot> slots, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
        MoneyInside = Guard.Against.Null(moneyInside);
        Slots = Guard.Against.Null(slots);
    }

    [Id(0)]
    public Guid Id { get; }

    [Id(1)]
    public Money MoneyInside { get; }

    [Id(2)]
    public IImmutableList<Slot> Slots { get; }
}

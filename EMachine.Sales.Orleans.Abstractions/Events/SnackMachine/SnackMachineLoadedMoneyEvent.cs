using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadedMoneyEvent : SnackMachineEvent
{
    public SnackMachineLoadedMoneyEvent(Guid id, Money money, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Money = Guard.Against.Null(money);
    }

    [Id(0)]
    public Money Money { get; }
}

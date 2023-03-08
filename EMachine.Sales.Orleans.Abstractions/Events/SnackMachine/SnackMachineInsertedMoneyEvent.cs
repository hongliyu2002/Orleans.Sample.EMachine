using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineInsertedMoneyEvent : SnackMachineEvent
{
    public SnackMachineInsertedMoneyEvent(Guid id, Money money, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Money = Guard.Against.Null(money);
    }

    [Id(0)]
    public Money Money { get; }
}

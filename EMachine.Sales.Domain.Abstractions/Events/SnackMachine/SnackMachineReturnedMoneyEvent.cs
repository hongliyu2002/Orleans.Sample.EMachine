using EMachine.Domain.Shared;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReturnedMoneyEvent : SnackMachineEvent
{
    public SnackMachineReturnedMoneyEvent(Guid id, Money money, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

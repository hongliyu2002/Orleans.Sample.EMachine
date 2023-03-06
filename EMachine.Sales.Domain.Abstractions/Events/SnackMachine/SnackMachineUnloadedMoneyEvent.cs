namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineUnloadedMoneyEvent : SnackMachineEvent
{
    public SnackMachineUnloadedMoneyEvent(Guid id, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineMoneyUnloadedEvent : SnackMachineEvent
{
    public SnackMachineMoneyUnloadedEvent(Guid id, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(id, traceId, operatedAt, operatedBy)
    {
    }
}

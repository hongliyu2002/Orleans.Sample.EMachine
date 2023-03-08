namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineMoneyReturnedEvent : SnackMachineEvent
{
    public SnackMachineMoneyReturnedEvent(Guid id, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(id, traceId, operatedAt, operatedBy)
    {
    }
}

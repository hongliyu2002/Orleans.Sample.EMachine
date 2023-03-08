namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemovedEvent : SnackMachineEvent
{
    public SnackMachineRemovedEvent(Guid id, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(id, traceId, operatedAt, operatedBy)
    {
    }
}

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemovedEvent : SnackMachineEvent
{
    public SnackMachineRemovedEvent(Guid id, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

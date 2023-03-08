namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemovedEvent : SnackMachineEvent
{
    public SnackMachineRemovedEvent(Guid key, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
    }
}

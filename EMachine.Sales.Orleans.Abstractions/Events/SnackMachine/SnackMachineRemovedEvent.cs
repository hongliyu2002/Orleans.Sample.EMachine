namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRemovedEvent : SnackMachineEvent
{
    public SnackMachineRemovedEvent(Guid uuId, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
    }
}

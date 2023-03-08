namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemovedEvent : SnackEvent
{
    public SnackRemovedEvent(Guid id, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

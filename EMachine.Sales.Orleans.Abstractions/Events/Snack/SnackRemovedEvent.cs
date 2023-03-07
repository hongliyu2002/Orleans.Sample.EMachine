namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemovedEvent : SnackEvent
{
    public SnackRemovedEvent(long id, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

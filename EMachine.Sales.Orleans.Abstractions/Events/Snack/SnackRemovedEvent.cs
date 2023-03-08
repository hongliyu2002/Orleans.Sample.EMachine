namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemovedEvent : SnackEvent
{
    public SnackRemovedEvent(Guid key, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
    }
}

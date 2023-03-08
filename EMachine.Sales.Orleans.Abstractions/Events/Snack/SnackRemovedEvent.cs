namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackRemovedEvent : SnackEvent
{
    public SnackRemovedEvent(Guid uuId, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
    }
}

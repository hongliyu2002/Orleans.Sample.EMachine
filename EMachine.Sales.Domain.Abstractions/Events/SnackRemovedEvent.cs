namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackRemovedEvent : SnackEvent
{
    /// <inheritdoc />
    public SnackRemovedEvent()
    {
    }

    /// <inheritdoc />
    public SnackRemovedEvent(Guid id, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
    }
}

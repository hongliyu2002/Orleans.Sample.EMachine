using Fluxera.Entity.DomainEvents;
using Fluxera.Guards;

namespace EMachine.Domain.Shared.Events;

[Immutable]
[GenerateSerializer]
public abstract class DomainEvent : IDomainEvent, ITraceable
{
    protected DomainEvent()
    {
        OperatedBy = string.Empty;
    }

    protected DomainEvent(Guid traceId, string operatedBy)
        : this()
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    /// <inheritdoc />
    [Id(0)]
    public Guid TraceId { get; }

    /// <inheritdoc />
    [Id(1)]
    public string OperatedBy { get; }
}

using Fluxera.Entity.DomainEvents;
using Fluxera.Guards;

namespace EMachine.Orleans.Shared.Events;

[Immutable]
[GenerateSerializer]
public abstract record DomainEvent : IDomainEvent, ITraceable
{
    protected DomainEvent(Guid traceId, string operatedBy)
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    [Id(0)]
    public Guid TraceId { get; }
    [Id(1)]
    public string OperatedBy { get; } = string.Empty;
}

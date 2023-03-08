using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackEvent : DomainEvent
{
    protected SnackEvent(Guid id, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid Id { get; } = Guid.Empty;
}

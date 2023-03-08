using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackEvent : DomainEvent
{
    protected SnackEvent(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid Id { get; }
}

using EMachine.Domain.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackEvent : DomainEvent
{
    protected SnackEvent(long id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = Guard.Against.Negative(id, nameof(id));
    }

    [Id(0)]
    public long Id { get; }
}

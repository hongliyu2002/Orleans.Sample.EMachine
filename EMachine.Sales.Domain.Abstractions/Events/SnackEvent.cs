using EMachine.Domain.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]

[GenerateSerializer]
public abstract class SnackEvent : DomainEvent
{
    protected SnackEvent()
    {
    }

    protected SnackEvent(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        ID = Guard.Against.Empty(id, nameof(id));
    }

    [Id(0)]
    public Guid ID { get; set; }
}

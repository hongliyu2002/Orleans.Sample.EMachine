using EMachine.Domain.Shared.Events;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackMachineEvent : DomainEvent
{
    protected SnackMachineEvent(Guid id, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Id = id;
    }

    [Id(0)]
    public Guid Id { get; }
}

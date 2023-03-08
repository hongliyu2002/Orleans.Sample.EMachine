using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackMachineEvent : DomainEvent
{
    protected SnackMachineEvent(Guid key, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        Key = Guard.Against.Empty(key, nameof(key));
    }

    [Id(0)]
    public Guid Key { get; }
}

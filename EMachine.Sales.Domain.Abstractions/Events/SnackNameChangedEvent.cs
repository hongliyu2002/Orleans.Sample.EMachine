using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackNameChangedEvent : SnackEvent
{
    public SnackNameChangedEvent(Guid id, string name, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; }
}

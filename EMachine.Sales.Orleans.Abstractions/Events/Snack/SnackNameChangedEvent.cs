using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackNameChangedEvent : SnackEvent
{
    public SnackNameChangedEvent(Guid uuId, string name, Guid traceId, string operatedBy)
        : base(uuId, traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; }
}

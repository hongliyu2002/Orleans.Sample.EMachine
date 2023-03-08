using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackInitializedEvent : SnackEvent
{
    public SnackInitializedEvent(Guid key, string name, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; }
}

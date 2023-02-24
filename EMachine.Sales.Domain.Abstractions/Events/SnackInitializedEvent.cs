using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Events;

[Immutable]
[Serializable]
[GenerateSerializer]
public sealed class SnackInitializedEvent : SnackEvent
{
    /// <inheritdoc />
    public SnackInitializedEvent()
    {
        Name = string.Empty;
    }

    /// <inheritdoc />
    public SnackInitializedEvent(Guid id, string name, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    [Id(0)]
    public string Name { get; set; }
}

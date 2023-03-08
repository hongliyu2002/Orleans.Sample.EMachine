using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineSnackBoughtEvent : SnackMachineEvent
{
    public SnackMachineSnackBoughtEvent(Guid id, int position, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(id, traceId, operatedAt, operatedBy)
    {
        Position = Guard.Against.Negative(position);
    }

    [Id(0)]
    public int Position { get; }
}

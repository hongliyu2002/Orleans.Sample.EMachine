using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineBoughtSnackEvent : SnackMachineEvent
{
    public SnackMachineBoughtSnackEvent(Guid id, int position, Guid traceId, string operatedBy)
        : base(id, traceId, operatedBy)
    {
        Position = Guard.Against.Negative(position);
    }

    [Id(0)]
    public int Position { get; }
}

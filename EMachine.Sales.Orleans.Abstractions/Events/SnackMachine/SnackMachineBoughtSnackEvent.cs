using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineBoughtSnackEvent : SnackMachineEvent
{
    public SnackMachineBoughtSnackEvent(Guid key, int position, Guid traceId, string operatedBy)
        : base(key, traceId, operatedBy)
    {
        Position = Guard.Against.Negative(position);
    }

    [Id(0)]
    public int Position { get; }
}

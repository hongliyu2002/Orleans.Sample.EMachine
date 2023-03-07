using EMachine.Orleans.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public abstract record SnackMachineCommand : ITraceable
{
    protected SnackMachineCommand(Guid traceId, string operatedBy)
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    [Id(0)]
    public Guid TraceId { get; }
    [Id(1)]
    public string OperatedBy { get; } = string.Empty;
}

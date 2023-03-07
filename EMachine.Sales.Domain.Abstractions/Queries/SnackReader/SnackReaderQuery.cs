using EMachine.Domain.Shared;
using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Queries;

[Immutable]
[GenerateSerializer]
public abstract record SnackReaderQuery : ITraceable
{
    protected SnackReaderQuery(Guid traceId, string operatedBy)
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    [Id(0)]
    public Guid TraceId { get; }

    [Id(1)]
    public string OperatedBy { get; } = string.Empty;
}

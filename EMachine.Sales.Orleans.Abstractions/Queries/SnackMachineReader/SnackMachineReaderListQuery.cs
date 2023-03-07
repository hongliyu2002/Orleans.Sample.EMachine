using Fluxera.Guards;

namespace EMachine.Sales.Orleans.Abstractions.Queries;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineReaderListQuery : SnackMachineReaderQuery
{
    public SnackMachineReaderListQuery(int maxResultCount, int skipCount, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        MaxResultCount = Guard.Against.NegativeOrZero(maxResultCount, nameof(maxResultCount));
        SkipCount = Guard.Against.Negative(skipCount, nameof(skipCount));
    }

    [Id(0)]
    public int MaxResultCount { get; }

    [Id(1)]
    public int SkipCount { get; }
}

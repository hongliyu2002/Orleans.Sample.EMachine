using Fluxera.Guards;

namespace EMachine.Sales.Domain.Abstractions.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepositoryGetListQuery : SnackRepositoryQuery
{
    public SnackRepositoryGetListQuery(int maxResultCount, int skipCount, Guid traceId, string operatedBy)
        : base(traceId, operatedBy)
    {
        MaxResultCount = Guard.Against.NegativeOrZero(maxResultCount, nameof(maxResultCount));
        SkipCount = Guard.Against.Negative(skipCount, nameof(skipCount));
    }

    /// <summary>
    ///     Maximum result count should be returned.
    ///     This is generally used to limit result count on paging.
    /// </summary>
    [Id(0)]
    public int MaxResultCount { get; }

    /// <summary>
    ///     Skip count (beginning of the page).
    /// </summary>
    [Id(1)]
    public int SkipCount { get; }
}

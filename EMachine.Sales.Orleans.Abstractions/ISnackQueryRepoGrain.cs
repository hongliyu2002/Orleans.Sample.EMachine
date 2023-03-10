using System.Collections.Immutable;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Domain;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackQueryRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackBaseView>>> ListPagedAsync(SnackPagedListQuery query);

    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackBaseView>>> SearchPagedAsync(SnackSearchPagedListQuery query);
}

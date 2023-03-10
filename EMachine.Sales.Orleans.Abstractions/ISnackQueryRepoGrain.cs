using System.Collections.Immutable;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Orleans.Views;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackQueryRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackBasic>>> ListPagedAsync(SnackPagedListQuery query);

    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackBasic>>> SearchPagedAsync(SnackSearchPagedListQuery query);
}

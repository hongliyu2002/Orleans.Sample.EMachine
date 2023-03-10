using System.Collections.Immutable;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Orleans.Views;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineQueryRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackMachineBasic>>> ListPagedAsync(SnackMachinePagedListQuery query);
}

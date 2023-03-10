using System.Collections.Immutable;
using EMachine.Sales.Domain;
using EMachine.Sales.Orleans.Queries;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineQueryRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ImmutableList<SnackMachineBaseView>>> ListPagedAsync(SnackMachinePagedListQuery query);
}

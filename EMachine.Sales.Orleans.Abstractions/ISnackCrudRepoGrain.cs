using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackCrudRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackGrain>> GetAsync(SnackCrudRepoGetOneCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackCrudRepoGetManyCommand command);

    Task<Result<bool>> CreateAsync(SnackCrudRepoCreateOneCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackCrudRepoDeleteOneCommand cmd);
}

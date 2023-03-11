using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackCrudRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackGrain>> GetAsync(SnackRepoGetCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackRepoGetManyCommand command);

    Task<Result<bool>> CreateAsync(SnackRepoCreateCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackRepoDeleteCommand cmd);
}

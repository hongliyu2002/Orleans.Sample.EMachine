using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineCrudRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineCrudRepoGetOneCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineCrudRepoGetManyCommand command);

    Task<Result<bool>> CreateAsync(SnackMachineCrudRepoCreateOneCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackMachineCrudRepoDeleteOneCommand cmd);
}

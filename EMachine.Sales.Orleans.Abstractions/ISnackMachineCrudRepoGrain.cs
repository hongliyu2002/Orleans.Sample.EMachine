using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineCrudRepoGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineRepoGetCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineRepoGetManyCommand command);

    Task<Result<bool>> CreateAsync(SnackMachineRepoCreateCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackMachineRepoDeleteCommand cmd);
}

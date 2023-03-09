using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineWriterGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineWriterGetOneCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineWriterGetMultipleCommand command);

    Task<Result<bool>> CreateAsync(SnackMachineWriterCreateOneCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackMachineWriterDeleteOneCommand cmd);
}

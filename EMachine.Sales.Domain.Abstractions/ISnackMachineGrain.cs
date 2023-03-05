using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackMachineGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<SnackMachine>> GetAsync();
    
    Task<Result> InitializeAsync(SnackMachineInitializeCommand cmd);
}

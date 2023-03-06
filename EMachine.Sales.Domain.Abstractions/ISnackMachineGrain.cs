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

    Task<Result> RemoveAsync(SnackMachineRemoveCommand cmd);

    Task<Result> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd);

    Task<Result> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd);

    Task<Result> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd);

    Task<Result> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd);

    Task<Result> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd);

    Task<Result> BuySnackAsync(SnackMachineBuySnackCommand cmd);
}

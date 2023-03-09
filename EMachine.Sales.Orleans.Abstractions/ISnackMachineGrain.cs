using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackMachineGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<SnackMachine>> GetAsync();

    [AlwaysInterleave]
    Task<Result<Money>> GetMoneyInsideAsync();

    [AlwaysInterleave]
    Task<Result<decimal>> GetAmountInTransactionAsync();

    [AlwaysInterleave]
    Task<Result<ImmutableList<Slot>>> GetSlotsAsync();

    [AlwaysInterleave]
    Task<bool> CanInitializeAsync();

    Task<Result<bool>> InitializeAsync(SnackMachineInitializeCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanRemoveAsync();

    Task<Result<bool>> RemoveAsync(SnackMachineRemoveCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanLoadMoneyAsync();

    Task<Result<bool>> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanUnloadMoneyAsync();

    Task<Result<bool>> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanInsertMoneyAsync();

    Task<Result<bool>> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanReturnMoneyAsync();

    Task<Result<bool>> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanLoadSnacksAsync();

    Task<Result<bool>> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanBuySnackAsync();

    Task<Result<bool>> BuySnackAsync(SnackMachineBuySnackCommand cmd);
}

using System.Collections.Immutable;
using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackMachineGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<Money>> GetMoneyInsideAsync();

    [AlwaysInterleave]
    Task<Result<decimal>> GetAmountInTransactionAsync();

    [AlwaysInterleave]
    Task<Result<ImmutableList<Slot>>> GetSlotsAsync();

    [AlwaysInterleave]
    Task<bool> CanInitializeAsync();

    Task<Result> InitializeAsync(SnackMachineInitializeCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanRemoveAsync();

    Task<Result> RemoveAsync(SnackMachineRemoveCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanLoadMoneyAsync();

    Task<Result> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanUnloadMoneyAsync();

    Task<Result> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanInsertMoneyAsync();

    Task<Result> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanReturnMoneyAsync();

    Task<Result> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanLoadSnacksAsync();

    Task<Result> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanBuySnackAsync();

    Task<Result> BuySnackAsync(SnackMachineBuySnackCommand cmd);
}

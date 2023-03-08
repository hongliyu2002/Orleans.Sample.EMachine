using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Extensions;
using EMachine.Sales.Orleans.Abstractions;
using EMachine.Sales.Orleans.Abstractions.Commands;
using EMachine.Sales.Orleans.Abstractions.Events;
using EMachine.Sales.Orleans.Abstractions.States;
using EMachine.Sales.Orleans.Rules;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Orleans;

[LogConsistencyProvider(ProviderName = "EventStore")]
[StorageProvider(ProviderName = "SalesStore")]
public sealed class SnackMachineGrain : EventSourcingGrain<SnackMachine>, ISnackMachineGrain
{
    private readonly ILogger<SnackMachineGrain> _logger;

    /// <inheritdoc />
    public SnackMachineGrain(ILogger<SnackMachineGrain> logger)
        : base("Default", "Sales.SnackMachines")
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<Money>> GetMoneyInsideAsync()
    {
        var uuId = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok()
                                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                                     .Ensure(State.IsCreated, $"Snack {uuId} is not initialized.")
                                     .Map(() => State.MoneyInside));
    }

    /// <inheritdoc />
    public Task<Result<decimal>> GetAmountInTransactionAsync()
    {
        var uuId = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok()
                                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                                     .Ensure(State.IsCreated, $"Snack {uuId} is not initialized.")
                                     .Map(() => State.AmountInTransaction));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<Slot>>> GetSlotsAsync()
    {
        var uuId = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok()
                                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                                     .Ensure(State.IsCreated, $"Snack {uuId} is not initialized.")
                                     .Map(() => State.Slots.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<bool> CanInitializeAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated == false);
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackMachineInitializeCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack machine {uuId} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInitializedEvent(uuId, cmd.MoneyInside, cmd.Slots, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanRemoveAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction == 0m);
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackMachineRemoveCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {uuId} still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineRemovedEvent(uuId, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanLoadMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineLoadedMoneyEvent(uuId, cmd.Money, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanUnloadMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction == 0m);
    }

    /// <inheritdoc />
    public Task<Result> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {uuId} is still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineUnloadedMoneyEvent(uuId, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanInsertMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(Money.CoinsAndNotes.Contains(cmd.Money), $"Only single coin or note should be inserted into the snack machine {uuId}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineSingleCoinOrNoteRequired.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInsertedMoneyEvent(uuId, cmd.Money, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanReturnMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction > 0m && State.MoneyInside.TryAllocate(State.AmountInTransaction, out _));
    }

    /// <inheritdoc />
    public Task<Result> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction > 0m, $"Snack machine {uuId} is not in transaction.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction, out _), $"Not enough change in the snack machine {uuId}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineReturnedMoneyEvent(uuId, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanLoadSnacksAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out _), $"Slot at position {cmd.Position} in the snack machine {uuId} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineLoadedSnacksEvent(uuId, cmd.Position, cmd.SnackPile, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanBuySnackAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> BuySnackAsync(SnackMachineBuySnackCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out var slot), $"Slot at position {cmd.Position} in the snack machine {uuId} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(slot!.SnackPile is { }, $"Snack pile of the slot at position {cmd.Position} in the snack machine {uuId} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineSlotSnackPileNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(slot.SnackPile!.TryPopOne(out _), $"Not enough snack in the snack pile of the slot at position {cmd.Position} in the snack machine {uuId}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineSlotSnackPileNotEnoughSnack.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction < slot.SnackPile.Price, $"Not enough money (￥{State.AmountInTransaction}) to buy the {slot.SnackPile.Snack} (￥{slot.SnackPile.Price}) in the snack machine {uuId}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotEnoughMoney.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction - slot.SnackPile.Price, out _), $"Not enough change in the snack machine {uuId} after purchase.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(uuId, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineBoughtSnackEvent(uuId, cmd.Position, cmd.TraceId, cmd.OperatedBy)));
    }
}

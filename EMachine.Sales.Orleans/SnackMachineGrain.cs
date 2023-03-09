using System.Collections.Immutable;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Extensions;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Rules;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Orleans;

[LogConsistencyProvider(ProviderName = Constants.LogConsistencyStoreName)]
[StorageProvider(ProviderName = Constants.SalesStoreName)]
public sealed class SnackMachineGrain : EventSourcingGrain<SnackMachine>, ISnackMachineGrain
{
    private readonly ILogger<SnackMachineGrain> _logger;

    /// <inheritdoc />
    public SnackMachineGrain(ILogger<SnackMachineGrain> logger)
        : base(Constants.StreamProviderName, Constants.SnackMachineNamespace)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<SnackMachine>> GetAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok(State).Ensure(State.IsCreated, $"Snack machine {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result<Money>> GetMoneyInsideAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok().Ensure(State.IsCreated, $"Snack machine {id} is not initialized.").Map(() => State.MoneyInside));
    }

    /// <inheritdoc />
    public Task<Result<decimal>> GetAmountInTransactionAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok().Ensure(State.IsCreated, $"Snack machine {id} is not initialized.").Map(() => State.AmountInTransaction));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<Slot>>> GetSlotsAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok().Ensure(State.IsCreated, $"Snack machine {id} is not initialized.").Map(() => State.Slots.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<long>> GetVersionAsync()
    {
        return Task.FromResult(Result.Ok((long)Version));
    }

    /// <inheritdoc />
    public Task<bool> CanInitializeAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated == false);
    }

    /// <inheritdoc />
    public Task<Result<bool>> InitializeAsync(SnackMachineInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated == false, $"Snack machine {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineExists.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineInitializedEvent(id, cmd.MoneyInside, cmd.Slots, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanRemoveAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction == 0m);
    }

    /// <inheritdoc />
    public Task<Result<bool>> RemoveAsync(SnackMachineRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {id} still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineInTransaction.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineRemovedEvent(id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanLoadMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineMoneyLoadedEvent(id, cmd.Money, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanUnloadMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction == 0m);
    }

    /// <inheritdoc />
    public Task<Result<bool>> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {id} is still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineInTransaction.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineMoneyUnloadedEvent(id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanInsertMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(Money.CoinsAndNotes.Contains(cmd.Money), $"Only single coin or note should be inserted into the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSingleCoinOrNoteRequired.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineMoneyInsertedEvent(id, cmd.Money, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanReturnMoneyAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated && State.AmountInTransaction > 0m && State.MoneyInside.TryAllocate(State.AmountInTransaction, out _));
    }

    /// <inheritdoc />
    public Task<Result<bool>> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.AmountInTransaction > 0m, $"Snack machine {id} is not in transaction.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInTransaction.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction, out _), $"Not enough change in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineMoneyReturnedEvent(id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanLoadSnacksAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out _), $"Slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineSnacksLoadedEvent(id, cmd.Position, cmd.SnackPile, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanBuySnackAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> BuySnackAsync(SnackMachineBuySnackCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out var slot), $"Slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(slot!.SnackPile is not null, $"Snack pile of the slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotSnackPileNotExists.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(slot.SnackPile!.TryPopOne(out _), $"Not enough snack in the snack pile of the slot at position {cmd.Position} in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotSnackPileNotEnoughSnack.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.AmountInTransaction < slot.SnackPile.Price, $"Not enough money (￥{State.AmountInTransaction}) to buy the {slot.SnackPile.Snack} (￥{slot.SnackPile.Price}) in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughMoney.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction - slot.SnackPile.Price, out _), $"Not enough change in the snack machine {id} after purchase.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindAsync(() => PublishAsync(new SnackMachineSnackBoughtEvent(id, cmd.Position, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }
}

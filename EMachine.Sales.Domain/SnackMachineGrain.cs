﻿using EMachine.Domain.Shared;
using EMachine.Domain.Shared.Extensions;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.Events;
using EMachine.Sales.Domain.Abstractions.States;
using EMachine.Sales.Domain.Rules;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Domain;

[LogConsistencyProvider(ProviderName = "EventStore")]
[StorageProvider(ProviderName = "SalesStore")]
public sealed class SnackMachineGrain : EventPublisherGrain<SnackMachine>, ISnackMachineGrain
{
    private readonly ILogger<SnackMachineGrain> _logger;

    /// <inheritdoc />
    public SnackMachineGrain(ILogger<SnackMachineGrain> logger)
        : base("Default", "Sales.SnackMachines")
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<SnackMachine>> GetAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok(State).Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.").Ensure(State.IsCreated, $"Snack machine {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackMachineInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack machine {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInitializedEvent(id, cmd.MoneyInside, cmd.Slots, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackMachineRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {id} still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineRemovedEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> LoadMoneyAsync(SnackMachineLoadMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineLoadedMoneyEvent(id, cmd.Money, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> UnloadMoneyAsync(SnackMachineUnloadMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {id} is still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineUnloadedMoneyEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(Money.CoinsAndNotes.Contains(cmd.Money), $"Only single coin or note should be inserted into the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSingleCoinOrNoteRequired.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInsertedMoneyEvent(id, cmd.Money, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> ReturnMoneyAsync(SnackMachineReturnMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction > 0m, $"Snack machine {id} is not in transaction.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInTransaction.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction, out _), $"Not enough change in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineReturnedMoneyEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> LoadSnacksAsync(SnackMachineLoadSnacksCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out _), $"Slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineLoadedSnacksEvent(id, cmd.Position, cmd.SnackPile, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> BuySnackAsync(SnackMachineBuySnackCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.TryGetSlot(cmd.Position, out var slot), $"Slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(slot!.SnackPile is { }, $"Snack pile of the slot at position {cmd.Position} in the snack machine {id} does not exist.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotSnackPileNotExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(slot.SnackPile!.TryPopOne(out _), $"Not enough snack in the snack pile of the slot at position {cmd.Position} in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineSlotSnackPileNotEnoughSnack.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction < slot.SnackPile.Price, $"Not enough money (￥{State.AmountInTransaction}) to buy the {slot.SnackPile.Snack} (￥{slot.SnackPile.Price}) in the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughMoney.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.MoneyInside.TryAllocate(State.AmountInTransaction - slot.SnackPile.Price, out _), $"Not enough change in the snack machine {id} after purchase.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, ErrorCodes.SnackMachineNotEnoughChange.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineBoughtSnackEvent(id, cmd.Position, cmd.TraceId, cmd.OperatedBy)));
    }
}

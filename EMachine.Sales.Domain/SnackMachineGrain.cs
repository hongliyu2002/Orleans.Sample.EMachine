using EMachine.Domain.Shared;
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
[StorageProvider(ProviderName = "SnackMachineStore")]
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
        return Task.FromResult(Result.Ok(State)
                                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already removed.")
                                     .Ensure(State.IsCreated, $"Snack machine {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackMachineInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack machine {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineAlreadyExists.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInitializedEvent(id, cmd.MoneyInside, cmd.Slots, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackMachineRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineIsNotInitialized.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.AmountInTransaction == 0m, $"Snack machine {id} still in transaction with amount {State.AmountInTransaction}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineInTransaction.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineRemovedEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> InsertMoneyAsync(SnackMachineInsertMoneyCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack machine {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack machine {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineIsNotInitialized.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(Money.CoinsAndNotes.Contains(cmd.Money), $"Only single coin or note should be inserted into the snack machine {id}.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackMachineErrorOccurredEvent(id, SnackMachineErrorCodes.SnackMachineSingleCoinOrNoteRequired.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackMachineInsertedMoneyEvent(id, cmd.Money, cmd.TraceId, cmd.OperatedBy)));
    }
}

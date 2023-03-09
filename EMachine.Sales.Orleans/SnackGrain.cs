﻿using EMachine.Orleans.Shared;
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

[LogConsistencyProvider(ProviderName = "EventStore")]
[StorageProvider(ProviderName = "SalesStore")]
public sealed class SnackGrain : EventSourcingGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;

    /// <inheritdoc />
    public SnackGrain(ILogger<SnackGrain> logger)
        : base(Constants.StreamProviderName, Constants.SnackNamespace)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<Snack>> GetAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok(State).Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.").Ensure(State.IsCreated, $"Snack {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result<string>> GetNameAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok().Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.").Ensure(State.IsCreated, $"Snack {id} is not initialized.").Map(() => State.Name));
    }

    /// <inheritdoc />
    public Task<bool> CanInitializeAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated == false);
    }

    /// <inheritdoc />
    public Task<Result<bool>> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackExists.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanRemoveAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackRemovedEvent(id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanChangeNameAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> ChangeNameAsync(SnackChangeNameCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }
}

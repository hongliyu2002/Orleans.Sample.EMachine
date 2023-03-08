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
public sealed class SnackGrain : EventSourcingGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;

    /// <inheritdoc />
    public SnackGrain(ILogger<SnackGrain> logger)
        : base("Default", "Sales.Snacks")
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<string>> GetNameAsync()
    {
        var uuId = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok()
                                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                                     .Ensure(State.IsCreated, $"Snack {uuId} is not initialized.")
                                     .Map(() => State.Name));
    }

    /// <inheritdoc />
    public Task<bool> CanInitializeAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated == false);
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackInitializeCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack {uuId} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackInitializedEvent(uuId, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanRemoveAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackRemoveCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackRemovedEvent(uuId, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<bool> CanChangeNameAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var uuId = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {uuId} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {uuId} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(uuId, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackNameChangedEvent(uuId, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }
}

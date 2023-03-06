using EMachine.Domain.Shared;
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
[StorageProvider(ProviderName = "SnackStore")]
public sealed class SnackGrain : EventPublisherGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;

    /// <inheritdoc />
    public SnackGrain(ILogger<SnackGrain> logger)
        : base("Default", "Sales.Snacks")
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<Snack>> GetAsync()
    {
        var id = this.GetPrimaryKeyLong();
        return Task.FromResult(Result.Ok(State)
                                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                                     .Ensure(State.IsCreated, $"Snack {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated == false, $"Snack {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackExists.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackRemovedEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToMessage(), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }
}

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

[LogConsistencyProvider(ProviderName = "SnackEventStore")]
[StorageProvider(ProviderName = "SnackStore")]
public sealed class SnackGrain : EventPublisherGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;

    /// <inheritdoc />
    public SnackGrain(ILogger<SnackGrain> logger)
        : base("Default", "Sales.Snacks")
    {
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public Task<Result<Snack>> GetAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok(State)
                                     .Ensure(State.IsDeleted == false, $"Snack {id} has already removed.")
                                     .Ensure(State.CreatedAt != null, $"Snack {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.CreatedAt == null, $"Snack {id} already exists.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackAlreadyExists.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.CreatedAt != null, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackIsNotInitialized.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKeyLong();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already removed.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .EnsureAsync(State.CreatedAt != null, $"Snack {id} is not initialized.")
                     .TapErrorAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackIsNotInitialized.Value, string.Join(';', errors.Select(e => e.ToString())), cmd.TraceId, cmd.OperatedBy)))
                     .BindAsync(() => PublishAsync(new SnackRemovedEvent(id, cmd.TraceId, cmd.OperatedBy)));
    }
}

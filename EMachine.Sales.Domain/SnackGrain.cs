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
        : base("MemoryStream", "Sales.Snacks")
    {
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc />
    public Task<Result<Snack>> GetAsync()
    {
        return Task.FromResult(Result.Ok(State)
                                     .Ensure(State.IsDeleted == false, "Snack is already removed.")
                                     .Ensure(State.CreatedAt != null, "Snack is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return State.IsDeleted switch
               {
                   true => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, $"Snack {id} has already removed.", cmd.TraceId, cmd.OperatedBy)),
                   false => (State.CreatedAt == null) switch
                            {
                                true => PublishAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)),
                                false => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackAlreadyExist.Value, $"Snack {id} already exist.", cmd.TraceId, cmd.OperatedBy))
                            }
               };
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return State.IsDeleted switch
               {
                   true => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, $"Snack {id} has already removed.", cmd.TraceId, cmd.OperatedBy)),
                   false => (State.CreatedAt == null) switch
                            {
                                true => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy)),
                                false => PublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy))
                            }
               };
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return State.IsDeleted switch
               {
                   true => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, $"Snack {id} has already removed.", cmd.TraceId, cmd.OperatedBy)),
                   false => (State.CreatedAt == null) switch
                            {
                                true => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy)),
                                false => PublishAsync(new SnackRemovedEvent(id, cmd.TraceId, cmd.OperatedBy))
                            }
               };
    }
}

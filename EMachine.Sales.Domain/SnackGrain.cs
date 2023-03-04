using EMachine.Domain.Shared;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.Events;
using EMachine.Sales.Domain.Rules;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Domain;

[LogConsistencyProvider(ProviderName = "EventStore")]
[StorageProvider(ProviderName = "EventStore")]
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
    public Task<Result> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return (State.CreatedAt == null) switch
               {
                   true => SaveAndPublishAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)),
                   false => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackAlreadyExist.Value, $"Snack {id} already exist.", cmd.TraceId, cmd.OperatedBy))
               };
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return (State.CreatedAt != null) switch
               {
                   true => SaveAndPublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)),
                   false => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy))
               };
    }

    /// <inheritdoc />
    public Task<Result> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return (State.CreatedAt != null) switch
               {
                   true => !State.IsDeleted switch
                           {
                               true => SaveAndPublishAsync(new SnackRemovedEvent(id, cmd.TraceId, cmd.OperatedBy)),
                               false => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackHasRemoved.Value, $"Snack {id} has already removed.", cmd.TraceId,
                                                                                      cmd.OperatedBy))
                           },
                   false => PublishErrorAsync(new SnackErrorOccurredEvent(id, SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy))
               };
    }
}

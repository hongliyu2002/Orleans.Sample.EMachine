using System.Threading.Tasks;
using EMachine.Domain.Shared;
using EMachine.Domain.Shared.Events;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.Events;
using EMachine.Sales.Domain.Rules;
using FluentResults;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans;

namespace EMachine.Sales.Domain;

public sealed class SnackGrain : EventSourcingGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;

    /// <inheritdoc />
    public SnackGrain(ILogger<SnackGrain> logger)
        : base("memory", "Sales.Snacks")
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
                   false => PublishErrorAsync(new ErrorOccurredEvent(SnackErrorCodes.SnackAlreadyExist.Value, $"Snack {id} already exist.", cmd.TraceId, cmd.OperatedBy))
               };
    }

    /// <inheritdoc />
    public Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return (State.CreatedAt != null) switch
               {
                   true => SaveAndPublishAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, cmd.OperatedBy)),
                   false => PublishErrorAsync(new ErrorOccurredEvent(SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy))
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
                               false => PublishErrorAsync(new ErrorOccurredEvent(SnackErrorCodes.SnackHasRemoved.Value, $"Snack {id} has already removed.", cmd.TraceId,
                                                                                 cmd.OperatedBy))
                           },
                   false => PublishErrorAsync(new ErrorOccurredEvent(SnackErrorCodes.SnackDoesNotExist.Value, $"Snack {id} does not exist.", cmd.TraceId, cmd.OperatedBy))
               };
    }
}

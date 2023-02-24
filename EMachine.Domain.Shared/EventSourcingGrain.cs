using System;
using System.Threading;
using System.Threading.Tasks;
using EMachine.Domain.Shared.Events;
using FlakeId;
using FluentResults;
using Fluxera.Guards;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers.Streams.Common;
using Orleans.Streams;

namespace EMachine.Domain.Shared;

public abstract class EventSourcingGrain<TState> : JournaledGrain<TState, DomainEvent>
    where TState : class, new()
{
    private readonly string _name;
    private readonly string _nameSpace;
    protected IAsyncStream<DomainEvent> _stream = null!;
    protected IStreamProvider _streamProvider = null!;

    /// <inheritdoc />
    protected EventSourcingGrain(string name, string nameSpace)
    {
        _name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        _nameSpace = Guard.Against.NullOrWhiteSpace(nameSpace, nameof(nameSpace));
    }

    /// <inheritdoc />
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _streamProvider = this.GetStreamProvider(_name);
        _stream = _streamProvider.GetStream<DomainEvent>(_nameSpace, this.GetPrimaryKey());
        return base.OnActivateAsync(cancellationToken);
    }

    protected async Task<Result> SaveAndPublishAsync(DomainEvent evt)
    {
        try
        {
            RaiseEvent(evt);
            await _stream.OnNextAsync(evt, new EventSequenceTokenV2(Id.Create()));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Error has occurred during save and publish event.", ex));
        }
    }

    protected async Task<Result> PublishErrorAsync(ErrorOccurredEvent evt)
    {
        try
        {
            await _stream.OnNextAsync(evt, new EventSequenceTokenV2(Id.Create()));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Error has occurred during publish error event.", ex));
        }
    }
}

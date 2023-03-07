using EMachine.Orleans.Shared.Events;
using FlakeId;
using FlakeId.Extensions;
using Fluxera.Guards;
using Orleans.EventSourcing;
using Orleans.FluentResults;
using Orleans.Providers.Streams.Common;
using Orleans.Streams;

namespace EMachine.Orleans.Shared;

public abstract class EventSourcingGrain<TState> : JournaledGrain<TState, DomainEvent>, IEventSourcingGrain
    where TState : class, new()
{
    protected readonly string _name;
    protected readonly string _nameSpace;
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

    protected async Task<Result> PublishAsync(DomainEvent evt)
    {
        try
        {
            if (!await RaiseConditionalEvent(evt))
            {
                return Result.Fail("Raise conditional event failed.");
            }
            await _stream.OnNextAsync(evt, new EventSequenceTokenV2(Id.Create().ToUnixTimeMilliseconds()));
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
            await _stream.OnNextAsync(evt, new EventSequenceTokenV2(Id.Create().ToUnixTimeMilliseconds()));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new ExceptionalError("Error has occurred during publish error event.", ex));
        }
    }
}

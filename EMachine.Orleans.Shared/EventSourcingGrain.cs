using EMachine.Orleans.Shared.Events;
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
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _streamProvider = this.GetStreamProvider(_name);
        _stream = _streamProvider.GetStream<DomainEvent>(_nameSpace, this.GetPrimaryKey());
    }

    protected Task<Result<bool>> PublishAsync(DomainEvent evt)
    {
        return Result.Ok()
                     .MapTryAsync(() => RaiseConditionalEvent(evt))
                     .EnsureAsync(success => success, "Raise conditional event failed.")
                     .TapTryAsync(() => _stream.OnNextAsync(evt, new EventSequenceTokenV2(Version)));
    }

    protected Task<Result> PublishErrorAsync(ErrorOccurredEvent evt)
    {
        return Result.Ok()
                     .TapTryAsync(() => _stream.OnNextAsync(evt, new EventSequenceTokenV2(Version)));
    }
}

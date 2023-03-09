using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;
using Orleans.EventSourcing;
using Orleans.FluentResults;
using Orleans.Providers.Streams.Common;
using Orleans.Runtime;
using Orleans.Streams;

namespace EMachine.Orleans.Shared;

public abstract class EventSourcingGrain<TState> : JournaledGrain<TState, DomainEvent>, IGrainWithGuidKey
    where TState : class, new()
{
    private readonly string _nameSpace;
    private readonly string _provider;
    private IAsyncStream<DomainEvent> _stream = null!;
    private IStreamProvider _streamProvider = null!;

    /// <inheritdoc />
    protected EventSourcingGrain(string provider, string nameSpace)
    {
        _provider = Guard.Against.NullOrWhiteSpace(provider, nameof(provider));
        _nameSpace = Guard.Against.NullOrWhiteSpace(nameSpace, nameof(nameSpace));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _streamProvider = this.GetStreamProvider(_provider);
        _stream = _streamProvider.GetStream<DomainEvent>(StreamId.Create(_nameSpace, this.GetPrimaryKey()));
    }

    protected Task<Result<bool>> PublishAsync(DomainEvent evt)
    {
        return Result.Ok()
                     .MapTryAsync(() => RaiseConditionalEvent(evt))
                     .EnsureAsync(success => success, "Raise conditional event failed.")
                     .TapTryAsync(() => _stream.OnNextAsync(evt with { Version = Version }, new EventSequenceTokenV2(Version)));
    }

    protected Task<Result> PublishErrorAsync(ErrorOccurredEvent evt)
    {
        return Result.Ok().TapTryAsync(() => _stream.OnNextAsync(evt, new EventSequenceTokenV2(Version)));
    }
}

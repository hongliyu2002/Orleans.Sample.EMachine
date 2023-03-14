using EMachine.Orleans.Abstractions.Events;
using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;
using Orleans.EventSourcing;
using Orleans.FluentResults;
using Orleans.Providers.Streams.Common;
using Orleans.Runtime;
using Orleans.Streams;

namespace EMachine.Orleans.Abstractions;

public abstract class EventSourcingGrain<TState> : JournaledGrain<TState, DomainEvent>, IGrainWithGuidKey
    where TState : class, new()
{
    private readonly string _nameSpace;
    private readonly string _provider;
    protected readonly IServiceScopeFactory _scopeFactory;
    protected AsyncServiceScope _scope;
    private IAsyncStream<DomainEvent> _stream = null!;
    private IStreamProvider _streamProvider = null!;

    /// <inheritdoc />
    protected EventSourcingGrain(string provider, string nameSpace, IServiceScopeFactory scopeFactory)
    {
        _provider = Guard.Against.NullOrWhiteSpace(provider, nameof(provider));
        _nameSpace = Guard.Against.NullOrWhiteSpace(nameSpace, nameof(nameSpace));
        _scopeFactory = Guard.Against.Null(scopeFactory, nameof(scopeFactory));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _streamProvider = this.GetStreamProvider(_provider);
        _stream = _streamProvider.GetStream<DomainEvent>(StreamId.Create(_nameSpace, this.GetPrimaryKey()));
        _scope = _scopeFactory.CreateAsyncScope();
    }

    /// <inheritdoc />
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        await _scope.DisposeAsync();
        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    protected abstract Task<bool> PersistAsync(DomainEvent evt);

    protected Task<Result<bool>> PublishPersistedAsync(DomainEvent evt)
    {
        return Result.Ok()
                     .MapTryAsync(() => RaiseConditionalEvent(evt))
                     .MapTryIfAsync(raised => raised, _ => PersistAsync(evt))
                     .TapTryAsync(() => _stream.OnNextAsync(evt with { Version = Version }, new EventSequenceTokenV2(Version)));
    }

    protected Task<Result> PublishErrorAsync(ErrorOccurredEvent evt)
    {
        return Result.Ok().TapTryAsync(() => _stream.OnNextAsync(evt, new EventSequenceTokenV2(Version)));
    }
}

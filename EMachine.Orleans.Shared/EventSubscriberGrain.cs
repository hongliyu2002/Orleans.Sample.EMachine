using EMachine.Orleans.Shared.Events;
using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Streams;

namespace EMachine.Orleans.Shared;

public abstract class EventSubscriberGrain : Grain, IEventSubscriberGrain
{
    protected readonly string _provider;
    protected readonly string _nameSpace;
    protected readonly IServiceScopeFactory _scopeFactory;
    protected AsyncServiceScope _scope;
    protected IAsyncStream<DomainEvent> _stream = null!;
    protected IStreamProvider _streamProvider = null!;
    protected StreamSubscriptionHandle<DomainEvent>? _streamSubscription;

    /// <inheritdoc />
    protected EventSubscriberGrain(string provider, string nameSpace, IServiceScopeFactory scopeFactory)
    {
        _provider = Guard.Against.NullOrWhiteSpace(provider, nameof(provider));
        _nameSpace = Guard.Against.NullOrWhiteSpace(nameSpace, nameof(nameSpace));
        _scopeFactory = Guard.Against.Null(scopeFactory, nameof(scopeFactory));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _scope = _scopeFactory.CreateAsyncScope();
        _streamProvider = this.GetStreamProvider(_provider);
        _stream = _streamProvider.GetStream<DomainEvent>(_nameSpace, this.GetPrimaryKey());
        _streamSubscription = await _stream.SubscribeAsync(HandleNextAsync, HandleExceptionAsync, HandCompleteAsync);
    }

    /// <inheritdoc />
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        await _streamSubscription!.UnsubscribeAsync();
        await _scope.DisposeAsync();
        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    protected abstract Task<bool> HandleNextAsync(DomainEvent evt, StreamSequenceToken token);

    protected abstract Task HandleExceptionAsync(Exception exception);

    protected abstract Task HandCompleteAsync();
}

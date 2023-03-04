using EMachine.Domain.Shared.Events;
using Fluxera.Guards;
using Orleans.Streams;

namespace EMachine.Domain.Shared;

public abstract class EventSubscriberGrain : Grain, IEventSubscriberGrain
{
    protected readonly string _name;
    protected readonly string _nameSpace;
    protected IAsyncStream<DomainEvent> _stream = null!;
    protected IStreamProvider _streamProvider = null!;
    protected StreamSubscriptionHandle<DomainEvent>? _streamSubscription;

    /// <inheritdoc />
    protected EventSubscriberGrain(string name, string nameSpace)
    {
        _name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        _nameSpace = Guard.Against.NullOrWhiteSpace(nameSpace, nameof(nameSpace));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _streamProvider = this.GetStreamProvider(_name);
        _stream = _streamProvider.GetStream<DomainEvent>(_nameSpace, this.GetPrimaryKey());
        _streamSubscription = await _stream.SubscribeAsync(HandleNextAsync, HandleExceptionAsync, HandCompleteAsync);
        await base.OnActivateAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        await _streamSubscription!.UnsubscribeAsync();
        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    protected abstract Task<bool> HandleNextAsync(DomainEvent evt, StreamSequenceToken token);

    protected abstract Task HandleExceptionAsync(Exception exception);

    protected abstract Task HandCompleteAsync();
}

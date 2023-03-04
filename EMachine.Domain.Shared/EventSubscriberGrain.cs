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

    /// <inheritdoc />
    protected EventSubscriberGrain(string name, string nameSpace)
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
}

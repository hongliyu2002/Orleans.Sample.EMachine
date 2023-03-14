using EMachine.Orleans.Shared.Events;
using EMachine.Orleans.Shared.States;
using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;
using Orleans.EventSourcing;

namespace EMachine.Orleans.Shared;

public abstract class EventSourcingRepoGrain<TRepo> : JournaledGrain<TRepo, DomainEvent>, IGrainWithGuidKey
    where TRepo : Repo, new()
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected AsyncServiceScope _scope;

    protected EventSourcingRepoGrain(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = Guard.Against.Null(scopeFactory, nameof(scopeFactory));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _scope = _scopeFactory.CreateAsyncScope();
    }

    /// <inheritdoc />
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        await _scope.DisposeAsync();
        await base.OnDeactivateAsync(reason, cancellationToken);
    }
}

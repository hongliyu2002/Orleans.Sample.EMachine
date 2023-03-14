using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Orleans.Abstractions;

public abstract class RepoGrain : Grain, IGrainWithGuidKey
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected AsyncServiceScope _scope;

    protected RepoGrain(IServiceScopeFactory scopeFactory)
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

using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Events;
using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Events;
using Fluxera.Guards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace EMachine.Sales.Orleans.Projection;

[ImplicitStreamSubscription(Constants.SnackNamespace)]
public sealed class SnackSubscriberGrain : EventSubscriberGrain
{
    private readonly ILogger<SnackSubscriberGrain> _logger;
    private SalesDbContext _dbContext = null!;

    public SnackSubscriberGrain(IServiceScopeFactory scopeFactory, ILogger<SnackSubscriberGrain> logger)
        : base(Constants.StreamProviderName, Constants.SnackNamespace, scopeFactory)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _dbContext = _scope.ServiceProvider.GetRequiredService<SalesDbContext>();
    }

    /// <inheritdoc />
    protected override Task HandleNextAsync(DomainEvent evt, StreamSequenceToken seq)
    {
        switch (evt)
        {
            case SnackInitializedEvent snackEvt:
                return ApplyEventAsync(snackEvt);
            // return ApplyEventWithRetryAndFallbackAsync(snackEvt, seq, (e, s, ct) => ApplyEventAsync((SnackInitializedEvent)e, s, ct));
            case SnackRemovedEvent snackEvt:
                return ApplyEventAsync(snackEvt);
            // return ApplyEventWithRetryAndFallbackAsync(snackEvt, seq, (e, s, ct) => ApplyEventAsync((SnackRemovedEvent)e, s, ct));
            case SnackNameChangedEvent snackEvt:
                return ApplyEventAsync(snackEvt);
            // return ApplyEventWithRetryAndFallbackAsync(snackEvt, seq, (e, s, ct) => ApplyEventAsync((SnackNameChangedEvent)e, s, ct));
            case ErrorOccurredEvent errorEvt:
                _logger.LogWarning(errorEvt.Message);
                return Task.CompletedTask;
            default:
                return Task.CompletedTask;
        }
    }

    /// <inheritdoc />
    protected override Task HandleExceptionAsync(Exception exception)
    {
        _logger.LogError(exception, exception.Message);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task HandCompleteAsync()
    {
        _logger.LogInformation($"Stream {Constants.SnackNamespace} is completed.");
        return Task.CompletedTask;
    }

    #region Apply

    private async Task<bool> ApplyEventAsync(SnackInitializedEvent evt, CancellationToken cancellationToken = default)
    {
        var snack = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snack == null)
        {
            snack = new Snack
                    {
                        Id = evt.Id,
                        Name = evt.Name,
                        CreatedAt = evt.OperatedAt,
                        CreatedBy = evt.OperatedBy,
                        Version = evt.Version
                    };
            await _dbContext.Snacks.AddAsync(snack, cancellationToken);
        }
        if (_dbContext.Entry(snack).State != EntityState.Added)
        {
            _logger.LogWarning($"Apply SnackInitializedEvent: Snack {evt.Id} is already in the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackRemovedEvent evt, CancellationToken cancellationToken = default)
    {
        var snack = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snack == null)
        {
            _logger.LogWarning($"Apply SnackRemovedEvent: Snack {evt.Id} does not exist in the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        if (snack.Version != evt.Version - 1)
        {
            _logger.LogWarning($"Apply SnackRemovedEvent: Snack {evt.Id} version {snack.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        snack.DeletedAt = evt.OperatedAt;
        snack.DeletedBy = evt.OperatedBy;
        snack.IsDeleted = true;
        snack.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackNameChangedEvent evt, CancellationToken cancellationToken = default)
    {
        var snack = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snack == null)
        {
            _logger.LogWarning($"Apply SnackNameChangedEvent: Snack {evt.Id} does not exist in the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        if (snack.Version != evt.Version - 1)
        {
            _logger.LogWarning($"Apply SnackNameChangedEvent: Snack {evt.Id} version {snack.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        snack.Name = evt.Name;
        snack.LastModifiedAt = evt.OperatedAt;
        snack.LastModifiedBy = evt.OperatedBy;
        snack.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyFullUpdateAsync(SnackEvent evt, CancellationToken cancellationToken = default)
    {
        var snackGrain = GrainFactory.GetGrain<ISnackGrain>(evt.Id);
        var snackInGrain = (await snackGrain.GetAsync()).ValueOrDefault;
        var snackVersion = (await snackGrain.GetVersionAsync()).ValueOrDefault;
        var snack = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInGrain == null)
        {
            if (snack == null)
            {
                return true;
            }
            _dbContext.Remove(snack);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        if (snack == null)
        {
            snack = new Snack();
            await _dbContext.Snacks.AddAsync(snack, cancellationToken);
        }
        snack.Id = snackInGrain.Id;
        snack.Name = snackInGrain.Name;
        snack.CreatedAt = snackInGrain.CreatedAt;
        snack.LastModifiedAt = snackInGrain.LastModifiedAt;
        snack.DeletedAt = snackInGrain.DeletedAt;
        snack.CreatedBy = snackInGrain.CreatedBy;
        snack.LastModifiedBy = snackInGrain.LastModifiedBy;
        snack.DeletedBy = snackInGrain.DeletedBy;
        snack.IsDeleted = snackInGrain.IsDeleted;
        snack.Version = snackVersion;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    // private async Task<bool> ApplyEventWithRetryAndFallbackAsync(SnackEvent evt, StreamSequenceToken seq, Func<SnackEvent, CancellationToken, Task<bool>> applyEvent, CancellationToken cancellationToken = default)
    // {
    //     var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
    //     var fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(async ct => await ApplyFullUpdateAsync(evt, ct));
    //     var policy = Policy.WrapAsync(retryPolicy, fallbackPolicy);
    //     return await policy.ExecuteAsync(async ct => await applyEvent(evt, ct), cancellationToken);
    // }

    #endregion

}

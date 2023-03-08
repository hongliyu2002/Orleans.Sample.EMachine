using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Events;
using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Events;
using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Polly;

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
    protected override async Task<bool> HandleNextAsync(DomainEvent evt, StreamSequenceToken token)
    {
        switch (evt)
        {
            case SnackInitializedEvent snackEvt:
                return await ApplyEventWithRetryAndFallbackAsync(snackEvt, (e, ct) => ApplyEventAsync((SnackInitializedEvent)e, ct));
            case SnackRemovedEvent snackEvt:
                return await ApplyEventWithRetryAndFallbackAsync(snackEvt, (e, ct) => ApplyEventAsync((SnackRemovedEvent)e, ct));
            case SnackNameChangedEvent snackEvt:
                return await ApplyEventWithRetryAndFallbackAsync(snackEvt, (e, ct) => ApplyEventAsync((SnackNameChangedEvent)e, ct));
            case ErrorOccurredEvent errorEvt:
                _logger.LogWarning(errorEvt.Message);
                return true;
            default:
                return false;
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
        return Task.CompletedTask;
    }

    #region Apply

    private async Task<bool> ApplyEventWithRetryAndFallbackAsync(SnackEvent evt, Func<SnackEvent, CancellationToken, Task<bool>> applyEvent, CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
        var fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(async cancelToken => await ApplyFallbackAsync(evt, cancelToken));
        var policy = Policy.WrapAsync(retryPolicy, fallbackPolicy);
        return await policy.ExecuteAsync(async cancelToken => await applyEvent(evt, cancelToken), cancellationToken);
    }

    private async Task<bool> ApplyEventAsync(SnackInitializedEvent evt, CancellationToken cancellationToken = default)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            var snack = new Snack
                        {
                            Id = evt.Id,
                            Name = evt.Name,
                            CreatedAt = evt.OperatedAt,
                            CreatedBy = evt.OperatedBy
                        };
            await _dbContext.Snacks.AddAsync(snack, cancellationToken);
        }
        var savedNumber = await _dbContext.SaveChangesAsync(cancellationToken);
        return savedNumber > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackRemovedEvent evt, CancellationToken cancellationToken = default)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            _logger.LogWarning($"Apply event SnackRemovedEvent: Snack {evt.Id} does not exist.");
            return false;
        }
        snackInDb.DeletedAt = evt.OperatedAt;
        snackInDb.DeletedBy = evt.OperatedBy;
        snackInDb.IsDeleted = true;
        var savedNumber = await _dbContext.SaveChangesAsync(cancellationToken);
        return savedNumber > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackNameChangedEvent evt, CancellationToken cancellationToken = default)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            _logger.LogWarning($"Apply event SnackNameChangedEvent: Snack {evt.Id} does not exist.");
            return false;
        }
        snackInDb.Name = evt.Name;
        snackInDb.LastModifiedAt = evt.OperatedAt;
        snackInDb.LastModifiedBy = evt.OperatedBy;
        var savedNumber = await _dbContext.SaveChangesAsync(cancellationToken);
        return savedNumber > 0;
    }

    private async Task ApplyFallbackAsync(SnackEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackGrain = GrainFactory.GetGrain<ISnackGrain>(evt.Id);
            var snackResult = await snackGrain.GetAsync();
            if (snackResult.IsFailed)
            {
                _logger.LogWarning($"Apply Fallback: Snack grain.{evt.Id} does not exist. Reasons: {snackResult}");
                return;
            }
            var snackInGrain = snackResult.Value;
            var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
            if (snackInDb == null)
            {
                snackInDb = new Snack();
                await _dbContext.Snacks.AddAsync(snackInDb, cancellationToken);
            }
            snackInDb.Id = snackInGrain.Id;
            snackInDb.Name = snackInGrain.Name;
            snackInDb.CreatedAt = snackInGrain.CreatedAt;
            snackInDb.LastModifiedAt = snackInGrain.LastModifiedAt;
            snackInDb.DeletedAt = snackInGrain.DeletedAt;
            snackInDb.CreatedBy = snackInGrain.CreatedBy;
            snackInDb.LastModifiedBy = snackInGrain.LastModifiedBy;
            snackInDb.DeletedBy = snackInGrain.DeletedBy;
            snackInDb.IsDeleted = snackInGrain.IsDeleted;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Apply Fallback: {ex.Message}");
        }
    }

    #endregion

}

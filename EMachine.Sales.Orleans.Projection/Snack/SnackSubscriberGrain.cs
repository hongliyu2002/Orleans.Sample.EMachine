using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Events;
using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Events;
using Fluxera.Guards;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace EMachine.Sales.Orleans.Projection;

public sealed class SnackSubscriberGrain : EventSubscriberGrain
{
    private readonly ILogger<SnackSubscriberGrain> _logger;
    private SalesDbContext _dbContext = null!;

    public SnackSubscriberGrain(IServiceScopeFactory scopeFactory, ILogger<SnackSubscriberGrain> logger)
        : base("Default", "Sales.Snacks", scopeFactory)
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
                return await ApplyEventAsync(snackEvt);
            case SnackRemovedEvent snackEvt:
                return await ApplyEventAsync(snackEvt);
            case SnackNameChangedEvent snackEvt:
                return await ApplyEventAsync(snackEvt);
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

    private async Task<bool> ApplyEventAsync(SnackInitializedEvent evt)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb != null)
        {
            _logger.LogWarning($"Snack {evt.Id} already exists.");
            return false;
        }
        var snack = new Snack
                    {
                        Id = evt.Id,
                        Name = evt.Name,
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = evt.OperatedBy
                    };
        await _dbContext.Snacks.AddAsync(snack);
        var savedNumber = await _dbContext.SaveChangesAsync();
        return savedNumber > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackRemovedEvent evt)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            _logger.LogWarning($"Snack {evt.Id} does not exist.");
            return false;
        }
        snackInDb.DeletedAt = DateTimeOffset.UtcNow;
        snackInDb.DeletedBy = evt.OperatedBy;
        snackInDb.IsDeleted = true;
        var savedNumber = await _dbContext.SaveChangesAsync();
        return savedNumber > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackNameChangedEvent evt)
    {
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            _logger.LogWarning($"Snack {evt.Id} does not exist.");
            return false;
        }
        snackInDb.Name = evt.Name;
        snackInDb.LastModifiedAt = DateTimeOffset.UtcNow;
        snackInDb.LastModifiedBy = evt.OperatedBy;
        var savedNumber = await _dbContext.SaveChangesAsync();
        return savedNumber > 0;
    }

    private async Task<bool> ApplyFallbackAsync(SnackEvent evt)
    {
        var snackGrain = GrainFactory.GetGrain<ISnackGrain>(evt.Id);
        var snackResult = await snackGrain.GetAsync();
        if (snackResult.IsFailed)
        {
            _logger.LogWarning($"Snack grain.{evt.Id} does not exist. Reasons: {snackResult}");
            return false;
        }
        var snackInGrain = snackResult.Value;
        var snackInDb = await _dbContext.Snacks.FindAsync(evt.Id);
        if (snackInDb == null)
        {
            snackInDb = new Snack();
            await _dbContext.Snacks.AddAsync(snackInDb);
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
        var savedNumber = await _dbContext.SaveChangesAsync();
        return savedNumber > 0;
    }

    #endregion

}

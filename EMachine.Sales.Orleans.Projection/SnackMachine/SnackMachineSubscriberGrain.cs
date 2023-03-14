using EMachine.Orleans.Abstractions;
using EMachine.Orleans.Abstractions.Events;
using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Mappers;
using Fluxera.Guards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace EMachine.Sales.Orleans.Projection;

[ImplicitStreamSubscription(Constants.SnackMachineNamespace)]
public sealed class SnackMachineSubscriberGrain : EventSubscriberGrain
{
    private readonly ILogger<SnackMachineSubscriberGrain> _logger;
    private SalesDbContext _dbContext = null!;

    public SnackMachineSubscriberGrain(IServiceScopeFactory scopeFactory, ILogger<SnackMachineSubscriberGrain> logger)
        : base(Constants.StreamProviderName, Constants.SnackMachineNamespace, scopeFactory)
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
            case SnackMachineInitializedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineRemovedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineMoneyLoadedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineMoneyUnloadedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineMoneyInsertedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineMoneyReturnedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineSnacksLoadedEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineSnackBoughtEvent snackMachineEvt:
                return ApplyEventAsync(snackMachineEvt);
            case SnackMachineErrorOccurredEvent snackMachineErrorEvt:
                _logger.LogWarning($"Received ErrorOccurredEvent: {string.Join(';', snackMachineErrorEvt.Reasons)}");
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
        _logger.LogInformation($"Stream {Constants.SnackMachineNamespace} is completed.");
        return Task.CompletedTask;
    }

    #region Try Get

    private static bool TryGetSlot(SnackMachine snackMachine, int position, out Slot? slot)
    {
        slot = snackMachine.Slots.FirstOrDefault(x => x.Position == position);
        return slot != null;
    }

    #endregion

    #region Apply

    private async Task<bool> ApplyEventAsync(SnackMachineInitializedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                snackMachine = new SnackMachine
                               {
                                   Id = evt.Id,
                                   MoneyInside = evt.MoneyInside.Map(),
                                   Slots = evt.Slots.Select(x => x.Map(evt.Id)).ToList(),
                                   SlotsCount = evt.Slots.Count,
                                   TotalPrice = evt.Slots.Where(s => s.SnackPile != null).Select(s => s.SnackPile!).Sum(sp => sp.TotalPrice),
                                   CreatedAt = evt.OperatedAt,
                                   CreatedBy = evt.OperatedBy,
                                   Version = evt.Version
                               };
                await _dbContext.SnackMachines.AddAsync(snackMachine, cancellationToken);
            }
            if (_dbContext.Entry(snackMachine).State == EntityState.Added)
            {
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            _logger.LogWarning($"Apply SnackMachineInitializedEvent: Snack machine {evt.Id} is already in the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineInitializedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineRemovedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineRemovedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineRemovedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.DeletedAt = evt.OperatedAt;
            snackMachine.DeletedBy = evt.OperatedBy;
            snackMachine.IsDeleted = true;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineRemovedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyLoadedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyLoadedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyLoadedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.MoneyInside += evt.Money.Map();
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyLoadedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyUnloadedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyUnloadedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyUnloadedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.MoneyInside = new Money();
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyUnloadedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyInsertedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyInsertedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyInsertedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.AmountInTransaction += evt.Money.Amount;
            snackMachine.MoneyInside += evt.Money.Map();
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyInsertedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyReturnedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyReturnedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineMoneyReturnedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (!snackMachine.MoneyInside.TryAllocate(snackMachine.AmountInTransaction, out var moneyAllocated))
            {
                _logger.LogWarning($"Apply SnackMachineMoneyReturnedEvent: Snack machine {evt.Id} cannot allocate money for change. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.MoneyInside -= moneyAllocated;
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyReturnedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineSnacksLoadedEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.Include(x => x.Slots).FirstOrDefaultAsync(x => x.Id == evt.Id, cancellationToken);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineSnacksLoadedEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineSnacksLoadedEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (!TryGetSlot(snackMachine, evt.Position, out var slot) || slot == null)
            {
                _logger.LogWarning($"Apply SnackMachineSnacksLoadedEvent: Snack machine {evt.Id} slot at position {evt.Position} could not be found. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            slot.SnackPile = evt.SnackPile.Map();
            snackMachine.TotalPrice = snackMachine.Slots.Where(s => s.SnackPile != null).Select(s => s.SnackPile!).Sum(sp => sp.TotalPrice);
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyReturnedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyEventAsync(SnackMachineSnackBoughtEvent evt, CancellationToken cancellationToken = default)
    {
        try
        {
            var snackMachine = await _dbContext.SnackMachines.Include(x => x.Slots).FirstOrDefaultAsync(x => x.Id == evt.Id, cancellationToken);
            if (snackMachine == null)
            {
                _logger.LogWarning($"Apply SnackMachineSnackBoughtEvent: Snack machine {evt.Id} does not exist in the database. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (snackMachine.Version != evt.Version - 1)
            {
                _logger.LogWarning($"Apply SnackMachineSnackBoughtEvent: Snack machine {evt.Id} version {snackMachine.Version}) in the database should be {evt.Version - 1}. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            if (!TryGetSlot(snackMachine, evt.Position, out var slot) || slot?.SnackPile == null || !slot.SnackPile.TryPopOne())
            {
                _logger.LogWarning($"Apply SnackMachineSnackBoughtEvent: Snack machine {evt.Id} slot at position {evt.Position} could not be found or snack pile is empty. Try to execute full update...");
                return await ApplyFullUpdateAsync(evt, cancellationToken);
            }
            snackMachine.TotalPrice = snackMachine.Slots.Where(s => s.SnackPile != null).Select(s => s.SnackPile!).Sum(sp => sp.TotalPrice);
            snackMachine.AmountInTransaction -= slot.SnackPile.Price;
            snackMachine.LastModifiedAt = evt.OperatedAt;
            snackMachine.LastModifiedBy = evt.OperatedBy;
            snackMachine.Version = evt.Version;
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Apply SnackMachineMoneyReturnedEvent: Exception is occurred when try to write data to the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
    }

    private async Task<bool> ApplyFullUpdateAsync(SnackMachineEvent evt, CancellationToken cancellationToken = default)
    {
        var attempts = 0;
        bool retryNeeded;
        do
        {
            try
            {
                var snackMachineGrain = GrainFactory.GetGrain<ISnackMachineGrain>(evt.Id);
                var snackMachineInGrain = (await snackMachineGrain.GetAsync()).ValueOrDefault;
                var snackMachineVersion = (await snackMachineGrain.GetVersionAsync()).ValueOrDefault;
                var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
                if (snackMachineInGrain == null)
                {
                    if (snackMachine == null)
                    {
                        return true;
                    }
                    _dbContext.Remove(snackMachine);
                    return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
                }
                if (snackMachine == null)
                {
                    snackMachine = new SnackMachine();
                    await _dbContext.SnackMachines.AddAsync(snackMachine, cancellationToken);
                }
                snackMachine.Id = snackMachineInGrain.Id;
                snackMachine.MoneyInside = snackMachineInGrain.MoneyInside.Map();
                snackMachine.AmountInTransaction = snackMachineInGrain.AmountInTransaction;
                snackMachine.Slots = snackMachineInGrain.Slots.Select(x => x.Map(snackMachineInGrain.Id)).ToList();
                snackMachine.SlotsCount = snackMachineInGrain.Slots.Count;
                snackMachine.TotalPrice = snackMachineInGrain.TotalPrice;
                snackMachine.CreatedAt = snackMachineInGrain.CreatedAt;
                snackMachine.LastModifiedAt = snackMachineInGrain.LastModifiedAt;
                snackMachine.DeletedAt = snackMachineInGrain.DeletedAt;
                snackMachine.CreatedBy = snackMachineInGrain.CreatedBy;
                snackMachine.LastModifiedBy = snackMachineInGrain.LastModifiedBy;
                snackMachine.DeletedBy = snackMachineInGrain.DeletedBy;
                snackMachine.IsDeleted = snackMachineInGrain.IsDeleted;
                snackMachine.Version = snackMachineVersion;
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                retryNeeded = ++attempts <= 3;
                if (retryNeeded)
                {
                    _logger.LogWarning($"Apply FullUpdate: DbUpdateConcurrencyException is occurred when try to write data to the database. Retrying {attempts}...");
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(1.5, attempts)), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Apply FullUpdate: Exception is occurred when try to write data to the database.");
                retryNeeded = false;
            }
        }
        while (retryNeeded);
        return false;
    }

    #endregion

}

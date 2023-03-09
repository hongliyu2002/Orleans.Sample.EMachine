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

[ImplicitStreamSubscription(Constants.SnackMachineNamespace)]
public sealed class SnackMachineSubscriberGrain : EventSubscriberGrain
{
    private readonly ILogger<SnackSubscriberGrain> _logger;
    private SalesDbContext _dbContext = null!;

    public SnackMachineSubscriberGrain(IServiceScopeFactory scopeFactory, ILogger<SnackSubscriberGrain> logger)
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
        _logger.LogInformation($"Stream {Constants.SnackNamespace} is completed.");
        return Task.CompletedTask;
    }

    #region Try Get

    public bool TryGetSlot(SnackMachine snackMachine, int position, out Slot? slot)
    {
        slot = snackMachine.Slots.FirstOrDefault(x => x.Position == position);
        return slot != null;
    }

    #endregion

    #region Map

    private Money? Map(States.Money? money)
    {
        return money switch
               {
                   null => null,
                   _ => new Money
                        {
                            Yuan1 = money.Yuan1,
                            Yuan2 = money.Yuan2,
                            Yuan5 = money.Yuan5,
                            Yuan10 = money.Yuan10,
                            Yuan20 = money.Yuan20,
                            Yuan50 = money.Yuan50,
                            Yuan100 = money.Yuan100,
                            Amount = money.Amount
                        }
               };
    }

    private SnackPile? Map(States.SnackPile? snackPile)
    {
        return snackPile switch
               {
                   null => null,
                   _ => new SnackPile
                        {
                            SnackId = snackPile.Snack.Id,
                            Quantity = snackPile.Quantity,
                            Price = snackPile.Price
                        }
               };
    }

    private Slot? Map(States.Slot? slot, Guid machineId)
    {
        return slot switch
               {
                   null => null,
                   _ => new Slot
                        {
                            MachineId = machineId,
                            Position = slot.Position,
                            SnackPile = Map(slot.SnackPile)
                        }
               };
    }

    #endregion

    #region Apply

    private async Task<bool> ApplyEventAsync(SnackMachineInitializedEvent evt, CancellationToken cancellationToken = default)
    {
        var snackMachine = await _dbContext.SnackMachines.FindAsync(evt.Id);
        if (snackMachine == null)
        {
            snackMachine = new SnackMachine
                           {
                               Id = evt.Id,
                               MoneyInside = Map(evt.MoneyInside)!,
                               Slots = evt.Slots.Select(x => Map(x, evt.Id)!).ToList(),
                               CreatedAt = evt.OperatedAt,
                               CreatedBy = evt.OperatedBy,
                               Version = evt.Version
                           };
            await _dbContext.SnackMachines.AddAsync(snackMachine, cancellationToken);
        }
        if (_dbContext.Entry(snackMachine).State != EntityState.Added)
        {
            _logger.LogWarning($"Apply SnackMachineInitializedEvent: Snack machine {evt.Id} is already in the database. Try to execute full update...");
            return await ApplyFullUpdateAsync(evt, cancellationToken);
        }
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackMachineRemovedEvent evt, CancellationToken cancellationToken = default)
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

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyLoadedEvent evt, CancellationToken cancellationToken = default)
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
        snackMachine.MoneyInside += Map(evt.Money)!;
        snackMachine.LastModifiedAt = evt.OperatedAt;
        snackMachine.LastModifiedBy = evt.OperatedBy;
        snackMachine.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyUnloadedEvent evt, CancellationToken cancellationToken = default)
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

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyInsertedEvent evt, CancellationToken cancellationToken = default)
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
        snackMachine.MoneyInside += Map(evt.Money)!;
        snackMachine.LastModifiedAt = evt.OperatedAt;
        snackMachine.LastModifiedBy = evt.OperatedBy;
        snackMachine.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackMachineMoneyReturnedEvent evt, CancellationToken cancellationToken = default)
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

    private async Task<bool> ApplyEventAsync(SnackMachineSnacksLoadedEvent evt, CancellationToken cancellationToken = default)
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
        slot.SnackPile = Map(evt.SnackPile);
        snackMachine.LastModifiedAt = evt.OperatedAt;
        snackMachine.LastModifiedBy = evt.OperatedBy;
        snackMachine.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyEventAsync(SnackMachineSnackBoughtEvent evt, CancellationToken cancellationToken = default)
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
        snackMachine.AmountInTransaction -= slot.SnackPile.Price;
        snackMachine.LastModifiedAt = evt.OperatedAt;
        snackMachine.LastModifiedBy = evt.OperatedBy;
        snackMachine.Version = evt.Version;
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> ApplyFullUpdateAsync(SnackMachineEvent evt, CancellationToken cancellationToken = default)
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
        snackMachine.MoneyInside = Map(snackMachineInGrain.MoneyInside)!;
        snackMachine.AmountInTransaction = snackMachineInGrain.AmountInTransaction;
        snackMachine.Slots = snackMachineInGrain.Slots.Select(x => Map(x, snackMachineInGrain.Id)!).ToList();
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

    #endregion

}

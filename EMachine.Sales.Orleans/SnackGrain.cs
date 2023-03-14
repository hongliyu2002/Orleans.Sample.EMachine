using EMachine.Orleans.Abstractions;
using EMachine.Orleans.Abstractions.Events;
using EMachine.Orleans.Abstractions.Extensions;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.EntityFrameworkCore;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Rules;
using EMachine.Sales.Orleans.States;
using Fluxera.Guards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Providers;

namespace EMachine.Sales.Orleans;

[LogConsistencyProvider(ProviderName = Constants.LogConsistencyStoreName)]
[StorageProvider(ProviderName = Constants.SalesStoreName)]
public sealed class SnackGrain : EventSourcingGrain<Snack>, ISnackGrain
{
    private readonly ILogger<SnackGrain> _logger;
    private SalesDbContext _dbContext = null!;

    /// <inheritdoc />
    public SnackGrain(IServiceScopeFactory scopeFactory, ILogger<SnackGrain> logger)
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
    public Task<Result<Snack>> GetAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok(State).Ensure(State.IsCreated, $"Snack {id} is not initialized."));
    }

    /// <inheritdoc />
    public Task<Result<string>> GetNameAsync()
    {
        var id = this.GetPrimaryKey();
        return Task.FromResult(Result.Ok().Ensure(State.IsCreated, $"Snack {id} is not initialized.").Map(() => State.Name));
    }

    /// <inheritdoc />
    public Task<Result<long>> GetVersionAsync()
    {
        return Task.FromResult(Result.Ok((long)Version));
    }

    /// <inheritdoc />
    public Task<bool> CanInitializeAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated == false);
    }

    /// <inheritdoc />
    public Task<Result<bool>> InitializeAsync(SnackInitializeCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated == false, $"Snack {id} already exists.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackExists.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.Name.Length <= 100, $"The name of snack {id} is too long.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNameTooLong.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindTryAsync(() => PublishPersistedAsync(new SnackInitializedEvent(id, cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanRemoveAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> RemoveAsync(SnackRemoveCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindTryAsync(() => PublishPersistedAsync(new SnackRemovedEvent(id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<bool> CanChangeNameAsync()
    {
        return Task.FromResult(State.IsDeleted == false && State.IsCreated);
    }

    /// <inheritdoc />
    public Task<Result<bool>> ChangeNameAsync(SnackChangeNameCommand cmd)
    {
        var id = this.GetPrimaryKey();
        return Result.Ok()
                     .Ensure(State.IsDeleted == false, $"Snack {id} has already been removed.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackRemoved.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.IsCreated, $"Snack {id} is not initialized.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNotInitialized.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .EnsureAsync(State.Name.Length <= 100, $"The name of snack {id} is too long.")
                     .TapErrorTryAsync(errors => PublishErrorAsync(new SnackErrorOccurredEvent(id, ErrorCodes.SnackNameTooLong.Value, errors.ToReasons(), cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .BindTryAsync(() => PublishPersistedAsync(new SnackNameChangedEvent(id, cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    protected override async Task<bool> PersistAsync(DomainEvent evt)
    {
        if (evt is not SnackEvent snackEvent)
        {
            return false;
        }
        var attempts = 0;
        bool retryNeeded;
        do
        {
            try
            {
                var snackInGrain = State;
                var snackInDb = await _dbContext.Snacks.FindAsync(snackEvent.Id);
                if (snackInGrain == null)
                {
                    if (snackInDb == null)
                    {
                        return true;
                    }
                    _dbContext.Remove(snackInDb);
                    return await _dbContext.SaveChangesAsync() > 0;
                }
                if (snackInDb == null)
                {
                    snackInDb = new Snack();
                    _dbContext.Snacks.Add(snackInDb);
                }
                snackInDb.Id = snackInGrain.Id;
                snackInDb.CreatedAt = snackInGrain.CreatedAt;
                snackInDb.CreatedBy = snackInGrain.CreatedBy;
                snackInDb.LastModifiedAt = snackInGrain.LastModifiedAt;
                snackInDb.LastModifiedBy = snackInGrain.LastModifiedBy;
                snackInDb.DeletedAt = snackInGrain.DeletedAt;
                snackInDb.DeletedBy = snackInGrain.DeletedBy;
                snackInDb.IsDeleted = snackInGrain.IsDeleted;
                snackInDb.Name = snackInGrain.Name;
                snackInDb.PictureUrl = snackInGrain.PictureUrl;
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                retryNeeded = ++attempts <= 3;
                if (retryNeeded)
                {
                    _logger.LogWarning($"DbUpdateConcurrencyException is occurred when try to write snack {snackEvent.Id} data to the database. Retrying {attempts}...");
                    await Task.Delay(TimeSpan.FromSeconds(attempts));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception is occurred when try to write snack {snackEvent.Id} data to the database.");
                retryNeeded = false;
            }
        }
        while (retryNeeded);
        return false;
    }
}

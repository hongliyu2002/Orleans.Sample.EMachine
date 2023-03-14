﻿using System.Collections.Immutable;
using System.Linq.Dynamic.Core;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Extensions;
using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Mappers;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Orleans.States;
using EMachine.Sales.Orleans.Views;
using Fluxera.Guards;
using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

[StatelessWorker]
public class SnackMachineRepoGrain : EventSourcingRepoGrain<SnackMachineRepo>, ISnackMachineRepoGrain
{
    private readonly ILogger<SnackMachineRepoGrain> _logger;
    private SalesDbContext _dbContext = null!;

    /// <inheritdoc />
    public SnackMachineRepoGrain(IServiceScopeFactory scopeFactory, ILogger<SnackMachineRepoGrain> logger)
        : base(scopeFactory)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        await base.OnActivateAsync(cancellationToken);
        _dbContext = _scope.ServiceProvider.GetRequiredService<SalesDbContext>();
    }

    #region Query Repo

    /// <inheritdoc />
    public Task<Result<ImmutableList<SnackMachineBasic>>> ListPagedAsync(SnackMachinePagedListQuery query)
    {
        return Result.Ok(_dbContext.SnackMachines.AsNoTracking().Where(x => x.IsDeleted == false))
                     .MapIf(query.IncludeSlots, snackMachines => snackMachines.Include(x => x.Slots))
                     .MapIf(query.Sorts.IsNotNullOrEmpty(), snackMachines => snackMachines.OrderBy(query.Sorts.ToSortStrinng()))
                     .Ensure(query.SkipCount >= 0, "Skip count should not be negative.")
                     .Map(snackMachines => snackMachines.Skip(query.SkipCount))
                     .Ensure(query.MaxResultCount >= 1, "Max result count should not be negative or zero.")
                     .Map(snackMachines => snackMachines.Take(query.MaxResultCount))
                     .Map(snackMachines => snackMachines.Select(x => new SnackMachineBasic(x.Id, x.MoneyInside.Map(), x.AmountInTransaction, x.SlotsCount, x.TotalPrice, x.Slots.Select(s => s.Map(x.Id)).ToImmutableList())))
                     .MapTryAsync(snackMachines => snackMachines.ToImmutableListAsync());
    }

    #endregion

    #region CRUD Repo

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineRepoGetCommand cmd)
    {
        return Result.Ok().EnsureAsync(() => _dbContext.SnackMachineIds.AnyAsync(x => x.Id == cmd.Id), $"Snack machine {cmd.Id} does not exist or has been deleted.").MapTryAsync(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineRepoGetManyCommand cmd)
    {
        return Result.Ok().MapTryAsync(() => _dbContext.SnackMachineIds.Select(x => x.Id).Intersect(cmd.Ids).ToListAsync()).MapTryAsync(ids => ids.Select(id => GrainFactory.GetGrain<ISnackMachineGrain>(id)).ToImmutableList());
    }

    /// <inheritdoc />
    public Task<Result<bool>> CreateAsync(SnackMachineRepoCreateCommand cmd)
    {
        async Task<bool> AddOrUpdateSnackMachineIdAsync(Guid id)
        {
            var snackMachineId = await _dbContext.SnackMachineIds.FindAsync(id);
            if (snackMachineId != null)
            {
                return true;
            }
            _dbContext.SnackMachineIds.Add(new SnackMachineId { Id = id });
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return Result.Ok()
                     .EnsureAsync(() => _dbContext.SnackMachineIds.AllAsync(x => x.Id != cmd.Id), $"Snack machine {cmd.Id} already exists.")
                     .MapTryAsync(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"Snack machine {cmd.Id} cannot be initialized.")
                     .BindTryAsync(grain => grain.InitializeAsync(new SnackMachineInitializeCommand(cmd.MoneyInside, cmd.Slots, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .MapTryIfAsync(initialized => initialized, _ => RaiseConditionalEvent(new SnackMachineRepoCreatedEvent(cmd.Id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .MapTryIfAsync(raised => raised, _ => AddOrUpdateSnackMachineIdAsync(cmd.Id));
    }

    /// <inheritdoc />
    public Task<Result<bool>> DeleteAsync(SnackMachineRepoDeleteCommand cmd)
    {
        async Task<bool> RemoveSnackMachineIdAsync(Guid id)
        {
            var snackMachineId = await _dbContext.SnackMachineIds.FindAsync(id);
            if (snackMachineId == null)
            {
                return true;
            }
            _dbContext.SnackMachineIds.Remove(snackMachineId);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        return Result.Ok()
                     .EnsureAsync(() => _dbContext.SnackMachineIds.AnyAsync(x => x.Id == cmd.Id), $"Snack machine {cmd.Id} does not exist or has been deleted.")
                     .MapTryAsync(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"Snack machine {cmd.Id} cannot be removed")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackMachineRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .MapTryIfAsync(removed => removed, _ => RaiseConditionalEvent(new SnackMachineRepoDeletedEvent(cmd.Id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)))
                     .MapTryIfAsync(raised => raised, _ => RemoveSnackMachineIdAsync(cmd.Id));
    }

    #endregion

}

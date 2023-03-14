using System.Collections.Immutable;
using System.Linq.Dynamic.Core;
using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Extensions;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Events;
using EMachine.Sales.Orleans.Queries;
using EMachine.Sales.Orleans.States;
using EMachine.Sales.Orleans.Views;
using Fluxera.Guards;
using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

// [StatelessWorker]
public class SnackRepoGrain : EventSourcingRepoGrain<SnackRepo>, ISnackRepoGrain
{
    private readonly ILogger<SnackRepoGrain> _logger;
    private SalesDbContext _dbContext = null!;

    /// <inheritdoc />
    public SnackRepoGrain(IServiceScopeFactory scopeFactory, ILogger<SnackRepoGrain> logger)
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
    public Task<Result<ImmutableList<SnackBasic>>> ListPagedAsync(SnackPagedListQuery query)
    {
        return Result.Ok(_dbContext.Snacks.AsNoTracking().Where(x => x.IsDeleted == false))
                     .MapIf(query.Sorts.IsNotNullOrEmpty(), snacks => snacks.OrderBy(query.Sorts.ToSortStrinng()))
                     .Ensure(query.SkipCount >= 0, "Skip count should not be negative.")
                     .Map(snacks => snacks.Skip(query.SkipCount))
                     .Ensure(query.MaxResultCount >= 1, "Max result count should not be negative or zero.")
                     .Map(snacks => snacks.Take(query.MaxResultCount))
                     .Map(snacks => snacks.Select(x => new SnackBasic(x.Id, x.Name)))
                     .MapTryAsync(snacks => snacks.ToImmutableListAsync());
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<SnackBasic>>> SearchPagedAsync(SnackSearchPagedListQuery query)
    {
        return Result.Ok(_dbContext.Snacks.AsNoTracking().Where(x => x.IsDeleted == false))
                     .Ensure(query.Criteria.IsNotNullOrEmpty(), "Criteria should not be empty.")
                     .Map(snacks => snacks.Where(x => x.Name.Contains(query.Criteria)))
                     .MapIf(query.Sorts.IsNotNullOrEmpty(), snacks => snacks.OrderBy(query.Sorts.ToSortStrinng()))
                     .Ensure(query.SkipCount >= 0, "Skip count should not be negative.")
                     .Map(snacks => snacks.Skip(query.SkipCount))
                     .Ensure(query.MaxResultCount >= 1, "Max result count should not be negative or zero.")
                     .Map(snacks => snacks.Take(query.MaxResultCount))
                     .Map(snacks => snacks.Select(x => new SnackBasic(x.Id, x.Name)))
                     .MapTryAsync(snacks => snacks.ToImmutableListAsync());
    }

    #endregion

    #region CRUD Repo

    /// <inheritdoc />
    public Task<Result<ISnackGrain>> GetAsync(SnackRepoGetCommand cmd)
    {
        return Task.FromResult(Result.Ok()
                                     .Ensure(State.Ids.Contains(cmd.Id), $"Snack {cmd.Id} does not exist or has been deleted.")
                                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackRepoGetManyCommand cmd)
    {
        var snacks = State.Ids.Intersect(cmd.Ids).Select(id => GrainFactory.GetGrain<ISnackGrain>(id));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<bool>> CreateAsync(SnackRepoCreateCommand cmd)
    {
        return Result.Ok()
                     .Ensure(State.Ids.Contains(cmd.Id) == false, $"Snack {cmd.Id} already exists.")
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"Snack {cmd.Id} cannot be initialized.")
                     .BindTryAsync(grain => grain.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .MapTryIfAsync(initialized => initialized, _ => RaiseConditionalEvent(new SnackRepoCreatedEvent(cmd.Id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    /// <inheritdoc />
    public Task<Result<bool>> DeleteAsync(SnackRepoDeleteCommand cmd)
    {
        return Result.Ok()
                     .Ensure(State.Ids.Contains(cmd.Id), $"Snack {cmd.Id} does not exist or has been deleted.")
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"Snack {cmd.Id} cannot be removed.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)))
                     .MapTryIfAsync(removed => removed, _ => RaiseConditionalEvent(new SnackRepoDeletedEvent(cmd.Id, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy, Version)));
    }

    #endregion

}

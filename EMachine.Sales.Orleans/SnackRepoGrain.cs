using System.Collections.Immutable;
using System.Linq.Dynamic.Core;
using EMachine.Orleans.Abstractions;
using EMachine.Orleans.Abstractions.Extensions;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.EntityFrameworkCore;
using EMachine.Sales.Orleans.Queries;
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
public class SnackRepoGrain : RepoGrain, ISnackRepoGrain
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
        return Result.Ok().EnsureAsync(() => _dbContext.Snacks.AnyAsync(x => x.Id == cmd.Id), $"Snack {cmd.Id} does not exist.").MapTryAsync(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackRepoGetManyCommand cmd)
    {
        return Result.Ok().MapTryAsync(() => _dbContext.Snacks.Select(x => x.Id).Intersect(cmd.Ids).ToListAsync()).MapTryAsync(ids => ids.Select(id => GrainFactory.GetGrain<ISnackGrain>(id)).ToImmutableList());
    }

    /// <inheritdoc />
    public Task<Result<bool>> CreateAsync(SnackRepoCreateCommand cmd)
    {
        return Result.Ok()
                     .EnsureAsync(() => _dbContext.Snacks.AllAsync(x => x.Id != cmd.Id), $"Snack {cmd.Id} already exists.")
                     .MapTryAsync(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"Snack {cmd.Id} cannot be initialized. It has already been deleted.")
                     .BindTryAsync(grain => grain.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result<bool>> DeleteAsync(SnackRepoDeleteCommand cmd)
    {
        return Result.Ok()
                     .EnsureAsync(() => _dbContext.Snacks.AnyAsync(x => x.Id == cmd.Id), $"Snack {cmd.Id} does not exist.")
                     .MapTryAsync(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"Snack {cmd.Id} cannot be removed. It has already been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    #endregion

}

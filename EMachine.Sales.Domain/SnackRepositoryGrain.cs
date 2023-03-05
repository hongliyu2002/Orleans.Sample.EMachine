using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Runtime;

namespace EMachine.Sales.Domain;

public class SnackRepositoryGrain : Grain, ISnackRepositoryGrain
{
    private readonly ILogger<SnackRepositoryGrain> _logger;
    private readonly IPersistentState<SnackRepository> _snacks;

    /// <inheritdoc />
    public SnackRepositoryGrain([PersistentState("SnackRepository", "SnackStore")] IPersistentState<SnackRepository> snacks, ILogger<SnackRepositoryGrain> logger)
    {
        _snacks = Guard.Against.Null(snacks, nameof(snacks));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<Snack>> GetSnackAsync(SnackRepositoryGetOneCommand cmd)
    {
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(cmd.Id), $"Snack {cmd.Id} does not exist.")
                     .BindAsync(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id)
                                                  .GetAsync());
    }

    /// <inheritdoc />
    public async Task<Result<IEnumerable<Snack>>> GetSnacksAsync(SnackRepositoryGetListCommand cmd)
    {
        var snackTasks = _snacks.State.Set.Select(id => GrainFactory.GetGrain<ISnackGrain>(id)
                                                                    .GetAsync());
        var snackResults = await Task.WhenAll(snackTasks.ToArray());
        var snacks = snackResults.Where(r => r is { IsSuccess: true, Value.IsDeleted: false })
                                 .Select(r => r.Value)
                                 .OrderBy(x => x.Name)
                                 .Skip(cmd.SkipCount)
                                 .Take(cmd.MaxResultCount);
        return Result.Ok(snacks);
    }

    /// <inheritdoc />
    public Task<Result<Snack>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd)
    {
        ISnackGrain? grain = null;
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(cmd.Id) == false, $"Snack {cmd.Id} already exists.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, cmd.OperatedBy)))
                     .TapAsync(() => _snacks.State.Set.Add(cmd.Id))
                     .BindAsync(() => grain!.GetAsync());
    }

    /// <inheritdoc />
    public Task<Result> DeleteSnackAsync(SnackRepositoryDeleteOneCommand cmd)
    {
        ISnackGrain? grain = null;
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(cmd.Id), $"Snack {cmd.Id} does not exist.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, cmd.OperatedBy)))
                     .TapAsync(() => _snacks.State.Set.Remove(cmd.Id));
    }

    /// <inheritdoc />
    public Task<Result> ChangeSnackNameAsync(SnackRepositoryChangeOneNameCommand cmd)
    {
        ISnackGrain? grain = null;
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(cmd.Id), $"Snack {cmd.Id} does not exist.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.ChangeNameAsync(new SnackNameChangeCommand(cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }
}

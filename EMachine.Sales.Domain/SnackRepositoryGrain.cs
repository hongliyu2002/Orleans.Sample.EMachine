using System.Collections.Immutable;
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
    public Task<Result<Snack>> GetSnackAsync(SnackRepositoryGetOneQuery query)
    {
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(query.Id), $"Snack {query.Id} does not exist.")
                     .BindAsync(() => GrainFactory.GetGrain<ISnackGrain>(query.Id)
                                                  .GetAsync());
    }

    /// <inheritdoc />
    public async Task<Result<ImmutableList<Snack>>> GetSnacksAsync(SnackRepositoryGetListQuery query)
    {
        var snackTasks = _snacks.State.Set.Select(id => GrainFactory.GetGrain<ISnackGrain>(id))
                                .Select(grain => grain.GetAsync());
        var snackResults = await Task.WhenAll(snackTasks.ToArray());
        var snacks = snackResults.Where(r => r is { IsSuccess: true, Value.IsDeleted: false })
                                 .Select(r => r.Value)
                                 .OrderBy(x => x.Name)
                                 .Skip(query.SkipCount)
                                 .Take(query.MaxResultCount);
        return Result.Ok(snacks.ToImmutableList());
    }

    /// <inheritdoc />
    public Task<Result<Snack>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd)
    {
        ISnackGrain? grain = null;
        return Result.Ok()
                     .Ensure(_snacks.State.Set.Contains(cmd.Id) == false, $"Snack {cmd.Id} already exists.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, cmd.OperatedBy)))
                     .TapTryAsync(() => _snacks.State.Set.Add(cmd.Id) ? _snacks.WriteStateAsync() : Task.CompletedTask)
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
                     .TapTryAsync(() => _snacks.State.Set.Remove(cmd.Id) ? _snacks.WriteStateAsync() : Task.CompletedTask);
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (_snacks.State.Set.Count == 0)
        {
            await Task.WhenAll(CreateSnackAsync(new SnackRepositoryCreateOneCommand(1, "Cafe", Guid.NewGuid(), "System")),
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(2, "Chocolate", Guid.NewGuid(), "System")),
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(3, "Soda", Guid.NewGuid(), "System")),
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(4, "Gum", Guid.NewGuid(), "System")));
        }
        await base.OnActivateAsync(cancellationToken);
    }
}

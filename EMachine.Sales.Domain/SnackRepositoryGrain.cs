using System.Collections.Immutable;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Runtime;

namespace EMachine.Sales.Domain;

public class SnackRepositoryGrain : Grain, ISnackRepositoryGrain
{
    private readonly ILogger<SnackRepositoryGrain> _logger;
    private readonly IPersistentState<HashSet<long>> _snacks;

    /// <inheritdoc />
    public SnackRepositoryGrain([PersistentState("SnackRepository", "SalesStore")] IPersistentState<HashSet<long>> snacks, ILogger<SnackRepositoryGrain> logger)
    {
        _snacks = Guard.Against.Null(snacks, nameof(snacks));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<ISnackGrain>> GetSnackAsync(SnackRepositoryGetOneQuery query)
    {
        return Task.FromResult(Result.Ok()
                                     .Ensure(_snacks.State.Contains(query.Id), $"Snack {query.Id} does not exist.")
                                     .Map(() => GrainFactory.GetGrain<ISnackGrain>(query.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackGrain>>> GetSnacksAsync(SnackRepositoryGetListQuery query)
    {
        var snacks = _snacks.State.Select(id => GrainFactory.GetGrain<ISnackGrain>(id))
                            .Skip(query.SkipCount)
                            .Take(query.MaxResultCount);
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<ISnackGrain>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd)
    {
        ISnackGrain? grain = default;
        return Result.Ok()
                     .Ensure(_snacks.State.Contains(cmd.Id) == false, $"Snack {cmd.Id} already exists.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, cmd.OperatedBy)))
                     .TapTryAsync(() => _snacks.State.Add(cmd.Id) ? _snacks.WriteStateAsync() : Task.CompletedTask)
                     .MapAsync(() => grain!);
    }

    /// <inheritdoc />
    public Task<Result> DeleteSnackAsync(SnackRepositoryDeleteOneCommand cmd)
    {
        ISnackGrain? grain = default;
        return Result.Ok()
                     .Ensure(_snacks.State.Contains(cmd.Id), $"Snack {cmd.Id} does not exist.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, cmd.OperatedBy)))
                     .TapTryAsync(() => _snacks.State.Remove(cmd.Id) ? _snacks.WriteStateAsync() : Task.CompletedTask);
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (_snacks.State.Count == 0)
        {
            await Task.WhenAll(CreateSnackAsync(new SnackRepositoryCreateOneCommand(1, "Cafe", Guid.NewGuid(), "System")), 
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(2, "Chocolate", Guid.NewGuid(), "System")), 
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(3, "Soda", Guid.NewGuid(), "System")),
                               CreateSnackAsync(new SnackRepositoryCreateOneCommand(4, "Gum", Guid.NewGuid(), "System")));
        }
        await base.OnActivateAsync(cancellationToken);
    }
}

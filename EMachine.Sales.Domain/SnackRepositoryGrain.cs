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
                                                                    .GetAsync())
                                .ToArray();
        var snackResults = await Task.WhenAll(snackTasks);
        var snacks = snackResults.Where(r => r is { IsSuccess: true, Value.IsDeleted: false })
                                 .Select(r => r.Value)
                                 .OrderBy(x => x.Name)
                                 .Skip(cmd.SkipCount)
                                 .Take(cmd.MaxResultCount);
        return Result.Ok(snacks);
    }
}

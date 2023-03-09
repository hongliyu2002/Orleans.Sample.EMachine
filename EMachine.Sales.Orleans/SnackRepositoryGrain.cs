using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

[StatelessWorker]
public class SnackRepositoryGrain : Grain, ISnackWriterGrain
{
    private readonly ILogger<SnackRepositoryGrain> _logger;

    /// <inheritdoc />
    public SnackRepositoryGrain(ILogger<SnackRepositoryGrain> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<ISnackGrain>> GetAsync(SnackWriterGetOneCommand cmd)
    {
        return Task.FromResult(Result.Ok(GrainFactory.GetGrain<ISnackGrain>(cmd.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackWriterGetMultipleCommand cmd)
    {
        var snacks = cmd.Ids.Select(id => GrainFactory.GetGrain<ISnackGrain>(id));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<bool>> CreateAsync(SnackWriterCreateOneCommand cmd)
    {
        return Result.Ok<ISnackGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"Snack {cmd.Id} already exists or has been deleted.")
                     .BindTryAsync(grain => grain.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result<bool>> DeleteAsync(SnackWriterDeleteOneCommand cmd)
    {
        return Result.Ok<ISnackGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"Snack {cmd.Id} does not exists or has been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }
}

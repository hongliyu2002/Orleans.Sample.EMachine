using System.Collections.Immutable;
using EMachine.Sales.Orleans.Abstractions;
using EMachine.Sales.Orleans.Abstractions.Commands;
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
        return Task.FromResult(Result.Ok(GrainFactory.GetGrain<ISnackGrain>(cmd.Key)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackWriterGetMultipleCommand cmd)
    {
        var snacks = cmd.Keys.Select(key => GrainFactory.GetGrain<ISnackGrain>(key));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<ISnackGrain>> CreateAsync(SnackWriterCreateOneCommand cmd)
    {
        return Result.Ok<ISnackGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Key))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"Snack {cmd.Key} already exists or has been deleted.")
                     .TapTryAsync(grain => grain.InitializeAsync(new SnackInitializeCommand(cmd.Name, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> DeleteAsync(SnackWriterDeleteOneCommand cmd)
    {
        return Result.Ok<ISnackGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackGrain>(cmd.Key))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"Snack {cmd.Key} does not exists or has been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackRemoveCommand(cmd.TraceId, cmd.OperatedBy)));
    }
}

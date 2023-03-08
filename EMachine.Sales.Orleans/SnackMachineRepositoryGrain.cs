using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

[StatelessWorker]
public class SnackMachineRepositoryGrain : Grain, ISnackMachineWriterGrain
{
    private readonly ILogger<SnackMachineRepositoryGrain> _logger;

    /// <inheritdoc />
    public SnackMachineRepositoryGrain(ILogger<SnackMachineRepositoryGrain> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineWriterGetOneCommand cmd)
    {
        return Task.FromResult(Result.Ok(GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineWriterGetMultipleCommand cmd)
    {
        var snacks = cmd.Ids.Select(id => GrainFactory.GetGrain<ISnackMachineGrain>(id));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> CreateAsync(SnackMachineWriterCreateOneCommand cmd)
    {
        return Result.Ok<ISnackMachineGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"SnackMachine {cmd.Id} already exists or has been deleted.")
                     .TapTryAsync(grain => grain.InitializeAsync(new SnackMachineInitializeCommand(cmd.MoneyInside, cmd.Slots, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> DeleteAsync(SnackMachineWriterDeleteOneCommand cmd)
    {
        return Result.Ok<ISnackMachineGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"SnackMachine {cmd.Id} does not exists or has been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackMachineRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }
}

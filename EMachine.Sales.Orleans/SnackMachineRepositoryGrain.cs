using System.Collections.Immutable;
using EMachine.Sales.Orleans.Abstractions;
using EMachine.Sales.Orleans.Abstractions.Commands;
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
        return Task.FromResult(Result.Ok(GrainFactory.GetGrain<ISnackMachineGrain>(cmd.UuId)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineWriterGetMultipleCommand cmd)
    {
        var snacks = cmd.UuIds.Select(uuId => GrainFactory.GetGrain<ISnackMachineGrain>(uuId));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> CreateAsync(SnackMachineWriterCreateOneCommand cmd)
    {
        return Result.Ok<ISnackMachineGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.UuId))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"SnackMachine {cmd.UuId} already exists or has been deleted.")
                     .TapTryAsync(grain => grain.InitializeAsync(new SnackMachineInitializeCommand(cmd.MoneyInside, cmd.Slots, cmd.TraceId, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result> DeleteAsync(SnackMachineWriterDeleteOneCommand cmd)
    {
        return Result.Ok<ISnackMachineGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.UuId))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"SnackMachine {cmd.UuId} does not exists or has been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackMachineRemoveCommand(cmd.TraceId, cmd.OperatedBy)));
    }
}

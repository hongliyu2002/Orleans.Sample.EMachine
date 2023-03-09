using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

[StatelessWorker]
public class SnackMachineRepoGrain : Grain, ISnackMachineCrudRepoGrain
{
    private readonly ILogger<SnackMachineRepoGrain> _logger;

    /// <inheritdoc />
    public SnackMachineRepoGrain(ILogger<SnackMachineRepoGrain> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineCrudRepoGetOneCommand cmd)
    {
        return Task.FromResult(Result.Ok(GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineCrudRepoGetManyCommand cmd)
    {
        var snacks = cmd.Ids.Select(id => GrainFactory.GetGrain<ISnackMachineGrain>(id));
        return Task.FromResult(Result.Ok(snacks.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<bool>> CreateAsync(SnackMachineCrudRepoCreateOneCommand cmd)
    {
        return Result.Ok()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanInitializeAsync(), $"SnackMachine {cmd.Id} already exists or has been deleted.")
                     .BindTryAsync(grain => grain.InitializeAsync(new SnackMachineInitializeCommand(cmd.MoneyInside, cmd.Slots, cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }

    /// <inheritdoc />
    public Task<Result<bool>> DeleteAsync(SnackMachineCrudRepoDeleteOneCommand cmd)
    {
        return Result.Ok<ISnackMachineGrain>()
                     .MapTry(() => GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .EnsureAsync(grain => grain.CanRemoveAsync(), $"SnackMachine {cmd.Id} does not exists or has been deleted.")
                     .BindTryAsync(grain => grain.RemoveAsync(new SnackMachineRemoveCommand(cmd.TraceId, DateTimeOffset.UtcNow, cmd.OperatedBy)));
    }
}

using System.Collections.Immutable;
using EMachine.Sales.Domain.Abstractions;
using EMachine.Sales.Domain.Abstractions.Commands;
using Fluxera.Guards;
using Microsoft.Extensions.Logging;
using Orleans.FluentResults;
using Orleans.Runtime;

namespace EMachine.Sales.Domain;

public class SnackMachineRepositoryGrain : Grain, ISnackMachineRepositoryGrain
{
    private readonly ILogger<SnackMachineRepositoryGrain> _logger;
    private readonly IPersistentState<HashSet<Guid>> _snackMachines;

    /// <inheritdoc />
    public SnackMachineRepositoryGrain([PersistentState("SnackMachineRepository", "SalesStore")] IPersistentState<HashSet<Guid>> snackMachines, ILogger<SnackMachineRepositoryGrain> logger)
    {
        _snackMachines = Guard.Against.Null(snackMachines, nameof(snackMachines));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> GetSnackMachineAsync(SnackMachineRepositoryGetOneQuery query)
    {
        return Task.FromResult(Result.Ok()
                                     .Ensure(_snackMachines.State.Contains(query.Id), $"Snack machine {query.Id} does not exist.")
                                     .Map(() => GrainFactory.GetGrain<ISnackMachineGrain>(query.Id)));
    }

    /// <inheritdoc />
    public Task<Result<ImmutableList<ISnackMachineGrain>>> GetSnackMachinesAsync(SnackMachineRepositoryGetListQuery query)
    {
        var snackMachines = _snackMachines.State.Select(id => GrainFactory.GetGrain<ISnackMachineGrain>(id))
                                          .Skip(query.SkipCount)
                                          .Take(query.MaxResultCount);
        return Task.FromResult(Result.Ok(snackMachines.ToImmutableList()));
    }

    /// <inheritdoc />
    public Task<Result<ISnackMachineGrain>> CreateSnackMachineAsync(SnackMachineRepositoryCreateOneCommand cmd)
    {
        ISnackMachineGrain? grain = default;
        return Result.Ok()
                     .Ensure(_snackMachines.State.Contains(cmd.Id) == false, $"Snack machine {cmd.Id} already exists.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.InitializeAsync(new SnackMachineInitializeCommand(cmd.MoneyInside, cmd.Slots, cmd.TraceId, cmd.OperatedBy)))
                     .TapTryAsync(() => _snackMachines.State.Add(cmd.Id) ? _snackMachines.WriteStateAsync() : Task.CompletedTask)
                     .MapAsync(() => grain!);
    }

    /// <inheritdoc />
    public Task<Result> DeleteSnackMachineAsync(SnackMachineRepositoryDeleteOneCommand cmd)
    {
        ISnackMachineGrain? grain = default;
        return Result.Ok()
                     .Ensure(_snackMachines.State.Contains(cmd.Id), $"Snack machine {cmd.Id} does not exist.")
                     .TapTry(() => grain = GrainFactory.GetGrain<ISnackMachineGrain>(cmd.Id))
                     .BindTryAsync(() => grain!.RemoveAsync(new SnackMachineRemoveCommand(cmd.TraceId, cmd.OperatedBy)))
                     .TapTryAsync(() => _snackMachines.State.Remove(cmd.Id) ? _snackMachines.WriteStateAsync() : Task.CompletedTask);
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (_snackMachines.State.Count == 0)
        {
            // await Task.WhenAll(CreateSnackMachineAsync(new SnackMachineRepositoryCreateOneCommand(1, "Cafe", Guid.NewGuid(), "System")), CreateSnackMachineAsync(new SnackMachineRepositoryCreateOneCommand(2, "Chocolate", Guid.NewGuid(), "System")),
            //                    CreateSnackMachineAsync(new SnackMachineRepositoryCreateOneCommand(3, "Soda", Guid.NewGuid(), "System")), CreateSnackMachineAsync(new SnackMachineRepositoryCreateOneCommand(4, "Gum", Guid.NewGuid(), "System")));
        }
        await base.OnActivateAsync(cancellationToken);
    }
}

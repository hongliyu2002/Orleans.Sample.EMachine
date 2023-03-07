using System.Collections.Immutable;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackMachineRepositoryGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackMachineGrain>> GetSnackMachineAsync(SnackMachineRepositoryGetOneQuery query);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackMachineGrain>>> GetSnackMachinesAsync(SnackMachineRepositoryGetListQuery query);

    Task<Result<ISnackMachineGrain>> CreateSnackMachineAsync(SnackMachineRepositoryCreateOneCommand cmd);

    Task<Result> DeleteSnackMachineAsync(SnackMachineRepositoryDeleteOneCommand cmd);
}

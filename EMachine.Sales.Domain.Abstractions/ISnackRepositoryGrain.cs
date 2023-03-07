using System.Collections.Immutable;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackRepositoryGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackGrain>> GetSnackAsync(SnackRepositoryGetOneQuery query);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackGrain>>> GetSnacksAsync(SnackRepositoryGetListQuery query);

    Task<Result<ISnackGrain>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd);

    Task<Result> DeleteSnackAsync(SnackRepositoryDeleteOneCommand cmd);
}

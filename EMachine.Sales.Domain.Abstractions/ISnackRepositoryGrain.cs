using System.Collections.Immutable;
using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackRepositoryGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<Snack>> GetSnackAsync(SnackRepositoryGetOneCommand cmd);

    [AlwaysInterleave]
    Task<Result<ImmutableList<Snack>>> GetSnacksAsync(SnackRepositoryGetListCommand cmd);

    Task<Result<Snack>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd);

    Task<Result> DeleteSnackAsync(SnackRepositoryDeleteOneCommand cmd);
    
    Task<Result> ChangeSnackNameAsync(SnackRepositoryChangeOneNameCommand cmd);
}

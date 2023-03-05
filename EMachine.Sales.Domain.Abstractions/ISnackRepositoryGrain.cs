using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackRepositoryGrain : IGrainWithGuidKey
{
    Task<Result<Snack>> GetSnackAsync(SnackRepositoryGetOneCommand cmd);

    Task<Result<IEnumerable<Snack>>> GetSnacksAsync(SnackRepositoryGetListCommand cmd);

    Task<Result<Snack>> CreateSnackAsync(SnackRepositoryCreateOneCommand cmd);

    Task<Result> DeleteSnackAsync(SnackRepositoryDeleteOneCommand cmd);
    
    Task<Result> ChangeSnackNameAsync(SnackRepositoryChangeOneNameCommand cmd);
}

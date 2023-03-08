using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.States;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<Snack>> GetAsync();
    
    [AlwaysInterleave]
    Task<Result<string>> GetNameAsync();

    [AlwaysInterleave]
    Task<bool> CanInitializeAsync();

    Task<Result> InitializeAsync(SnackInitializeCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanRemoveAsync();

    Task<Result> RemoveAsync(SnackRemoveCommand cmd);

    [AlwaysInterleave]
    Task<bool> CanChangeNameAsync();
    
    Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd);
}

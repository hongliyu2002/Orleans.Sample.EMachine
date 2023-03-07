using EMachine.Sales.Orleans.Abstractions.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans.Abstractions;

public interface ISnackGrain : IGrainWithIntegerKey
{
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

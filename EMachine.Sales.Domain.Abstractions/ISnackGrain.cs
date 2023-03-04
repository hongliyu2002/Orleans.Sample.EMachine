using EMachine.Sales.Domain.Abstractions.Commands;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackGrain : IGrainWithGuidKey
{
    Task<Result> InitializeAsync(SnackInitializeCommand cmd);

    Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd);

    Task<Result> RemoveAsync(SnackRemoveCommand cmd);
}

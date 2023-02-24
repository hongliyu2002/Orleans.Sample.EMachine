using System.Threading.Tasks;
using EMachine.Sales.Domain.Abstractions.Commands;
using FluentResults;
using Orleans;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackGrain : IGrainWithGuidKey
{
    Task<Result> InitializeAsync(SnackInitializeCommand cmd);

    Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd);

    Task<Result> RemoveAsync(SnackRemoveCommand cmd);
}

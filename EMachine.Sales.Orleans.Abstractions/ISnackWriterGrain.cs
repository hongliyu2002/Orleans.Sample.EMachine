using System.Collections.Immutable;
using EMachine.Sales.Orleans.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans;

public interface ISnackWriterGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackGrain>> GetAsync(SnackWriterGetOneCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackWriterGetMultipleCommand command);

    Task<Result<bool>> CreateAsync(SnackWriterCreateOneCommand cmd);

    Task<Result<bool>> DeleteAsync(SnackWriterDeleteOneCommand cmd);
}

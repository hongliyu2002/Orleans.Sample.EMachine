﻿using System.Collections.Immutable;
using EMachine.Sales.Orleans.Abstractions.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans.Abstractions;

public interface ISnackWriterGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackGrain>> GetAsync(SnackWriterGetOneCommand command);
    
    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackGrain>>> GetMultipleAsync(SnackWriterGetMultipleCommand command);

    Task<Result<ISnackGrain>> CreateAsync(SnackWriterCreateOneCommand cmd);

    Task<Result> DeleteAsync(SnackWriterDeleteOneCommand cmd);
}
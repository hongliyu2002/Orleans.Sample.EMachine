﻿using System.Collections.Immutable;
using EMachine.Sales.Orleans.Abstractions.Commands;
using Orleans.Concurrency;
using Orleans.FluentResults;

namespace EMachine.Sales.Orleans.Abstractions;

public interface ISnackMachineWriterGrain : IGrainWithGuidKey
{
    [AlwaysInterleave]
    Task<Result<ISnackMachineGrain>> GetAsync(SnackMachineWriterGetOneCommand command);

    [AlwaysInterleave]
    Task<Result<ImmutableList<ISnackMachineGrain>>> GetMultipleAsync(SnackMachineWriterGetMultipleCommand command);

    Task<Result<ISnackMachineGrain>> CreateAsync(SnackMachineWriterCreateOneCommand cmd);

    Task<Result> DeleteAsync(SnackMachineWriterDeleteOneCommand cmd);
}
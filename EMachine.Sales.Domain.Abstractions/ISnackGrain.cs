﻿using EMachine.Sales.Domain.Abstractions.Commands;
using EMachine.Sales.Domain.Abstractions.States;
using Orleans.FluentResults;

namespace EMachine.Sales.Domain.Abstractions;

public interface ISnackGrain : IGrainWithGuidKey
{
    Task<Result<Snack>> GetAsync();
    
    Task<Result> InitializeAsync(SnackInitializeCommand cmd);

    Task<Result> ChangeNameAsync(SnackNameChangeCommand cmd);

    Task<Result> RemoveAsync(SnackRemoveCommand cmd);
}

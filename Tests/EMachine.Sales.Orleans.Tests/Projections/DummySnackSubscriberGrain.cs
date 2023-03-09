using EMachine.Orleans.Shared;
using EMachine.Orleans.Shared.Events;
using EMachine.Sales.Orleans.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Xunit.Abstractions;

namespace EMachine.Sales.Orleans.Tests.Projections;

public interface IDummySnackSubscriberGrain : IGrainWithGuidKey
{
    Task Start();
    Task Stop();
    Task<string> GetCurrentState();
}

[ImplicitStreamSubscription(Constants.SnackNamespace)]
public class DummySnackSubscriberGrain : EventSubscriberGrain
{
    private readonly ILogger<DummySnackSubscriberGrain> _logger;
    private readonly ITestOutputHelper _output;

    public DummySnackSubscriberGrain(IServiceScopeFactory scopeFactory, ILogger<DummySnackSubscriberGrain> logger, ITestOutputHelper output)
        : base(Constants.StreamProviderName, Constants.SnackNamespace, scopeFactory)
    {
        _logger = logger;
        _output = output;
    }
    
    public Task Start()
    {
        _logger.LogInformation($"Grain {this.GetPrimaryKey()} started.");
        return Task.CompletedTask;
    }

    public Task Stop()
    {
        _logger.LogInformation($"Grain {this.GetPrimaryKey()} stopped.");
        return Task.CompletedTask;
    }

    public Task<string> GetCurrentState()
    {
        return Task.FromResult("Running");
    }
    
    /// <inheritdoc />
    protected override Task HandleNextAsync(DomainEvent evt, StreamSequenceToken token)
    {
        _logger.LogInformation($"Received Domain Event for grain {this.GetPrimaryKey()}.");
        switch (evt)
        {
            case SnackInitializedEvent snackEvt:
                _logger.LogInformation("Id: {0} Operator: {1} Name: {2}", snackEvt.Id, snackEvt.OperatedBy, snackEvt.Name);
                break;
            case SnackRemovedEvent snackEvt:
                _logger.LogInformation("Id: {0} Operator: {1}", snackEvt.Id, snackEvt.OperatedBy);
                break;
            case SnackNameChangedEvent snackEvt:
                _logger.LogInformation("Id: {0} Operator: {1} Name: {2}", snackEvt.Id, snackEvt.OperatedBy, snackEvt.Name);
                break;
            case ErrorOccurredEvent errorEvt:
                _logger.LogWarning("Code: {0} Message: {1} Operator: {2}", errorEvt.Code, string.Join(';', errorEvt.Reasons), errorEvt.OperatedBy);
                break;
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task HandleExceptionAsync(Exception exception)
    {
        _output.WriteLine(exception.Message);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override Task HandCompleteAsync()
    {
        _output.WriteLine("Completed!");
        return Task.CompletedTask;
    }
}

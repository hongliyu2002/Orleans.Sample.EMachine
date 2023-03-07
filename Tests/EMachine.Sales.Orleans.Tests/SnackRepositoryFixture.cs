using EMachine.Sales.Orleans.Abstractions;
using EMachine.Sales.Orleans.Abstractions.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using Orleans.TestingHost;
using Xunit;

namespace EMachine.Sales.Orleans.Tests;

public class SnackRepositoryFixture : IAsyncLifetime
{
    private readonly TestCluster _cluster;

    public SnackRepositoryFixture(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        var grain = _cluster.GrainFactory.GetGrain<ISnackWriterGrain>(Guid.Empty);
        await Task.WhenAll(grain.CreateAsync(new SnackWriterCreateOneCommand(1, "Cafe", Guid.NewGuid(), "System")), grain.CreateAsync(new SnackWriterCreateOneCommand(2, "Chocolate", Guid.NewGuid(), "System")),
                           grain.CreateAsync(new SnackWriterCreateOneCommand(3, "Soda", Guid.NewGuid(), "System")), grain.CreateAsync(new SnackWriterCreateOneCommand(4, "Gum", Guid.NewGuid(), "System")));
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

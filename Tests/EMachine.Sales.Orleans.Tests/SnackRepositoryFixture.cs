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
        await Task.WhenAll(grain.CreateAsync(new SnackWriterCreateOneCommand(new Guid("ae9e8d38-8289-47fe-8084-99df2b894556"), "Cafe", Guid.NewGuid(), "System")), 
                           grain.CreateAsync(new SnackWriterCreateOneCommand(new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122"), "Chocolate", Guid.NewGuid(), "System")),
                           grain.CreateAsync(new SnackWriterCreateOneCommand(new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629"), "Soda", Guid.NewGuid(), "System")), 
                           grain.CreateAsync(new SnackWriterCreateOneCommand(new Guid("ad63bc13-5075-47d7-8525-b32b52352192"), "Gum", Guid.NewGuid(), "System")));
    }

    /// <inheritdoc />
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

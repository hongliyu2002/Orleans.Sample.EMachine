using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.EntityFrameworkCore;
using EMachine.Sales.Orleans.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;
using Xunit;

namespace EMachine.Sales.Orleans.Tests;

public class SnackMachineFixture : IAsyncLifetime
{
    private readonly TestCluster _cluster;
    private SalesDbContext _dbContext = null!;

    public SnackMachineFixture(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        _dbContext = _cluster.ServiceProvider.GetRequiredService<SalesDbContext>();
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();
        var grain = _cluster.GrainFactory.GetGrain<ISnackCrudRepoGrain>(Guid.Empty);
        await Task.WhenAll(grain.CreateAsync(new SnackRepoCreateCommand(new Guid("ae9e8d38-8289-47fe-8084-99df2b894556"), "Cafe", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("23697d49-75f1-4e3c-aa0d-5a98cf3ad122"), "Chocolate", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("5b4103f4-7d90-4680-afc1-70dc48b96629"), "Soda", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("ad63bc13-5075-47d7-8525-b32b52352192"), "Gum", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")));
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await Task.Delay(1000);
        // await _dbContext.DisposeAsync();
    }
}

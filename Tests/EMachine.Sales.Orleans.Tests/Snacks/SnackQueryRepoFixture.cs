using EMachine.Sales.EntityFrameworkCore.Contexts;
using EMachine.Sales.Orleans.Commands;
using EMachine.Sales.Orleans.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestingHost;
using Xunit;

namespace EMachine.Sales.Orleans.Tests;

public class SnackQueryRepoFixture : IAsyncLifetime
{
    private readonly TestCluster _cluster;
    private SalesDbContext _dbContext = null!;

    public SnackQueryRepoFixture(ClusterFixture fixture)
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
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("ad63bc13-5075-47d7-8525-b32b52352192"), "Gum", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("bab70d70-26f1-4392-aa49-e3b3715bab62"), "Juice", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("8039f1f6-bcfc-4ade-b00d-f43b19bf728d"), "Energy Drinks", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("123def43-c176-463d-a4d4-82bfe71dff07"), "Water", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("89155377-9570-4bab-a552-ee0590705f02"), "Chips", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("1e9e8f16-6a9a-447d-a8a2-9812279316a7"), "Candy", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("5a68b6b9-e3ef-4289-b5b9-e549587deecc"), "Cookies", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("7013de20-3db6-4f8f-acdf-93eddf663a9a"), "Nuts", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")),
                           grain.CreateAsync(new SnackRepoCreateCommand(new Guid("d825e0bd-5b26-408b-856a-6d444be78090"), "Crackers", Guid.NewGuid(), DateTimeOffset.UtcNow, "System")));
        await Task.Delay(5000);
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await Task.Delay(2000);
        await _dbContext.DisposeAsync();
    }
}

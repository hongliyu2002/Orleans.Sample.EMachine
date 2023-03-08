using EMachine.Sales.Domain.Entities;
using EMachine.Sales.Domain.Repositories;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using FluentAssertions;
using Fluxera.Extensions.Hosting.Modules.UnitTesting;
using Fluxera.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.EntityFrameworkCore.Tests;

[Collection("Sales")]
public class SalesModuleTests : StartupModuleTestBase<SalesEntityFrameworkCoreModule>, IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SalesModuleTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        StartApplication();
        var dbContext = ApplicationLoader.ServiceProvider.GetRequiredService<SalesDbContext>();
        dbContext.Database.EnsureCreated();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        StopApplication();
    }

    [Fact]
    public async Task Should_Add_SnackEntity()
    {
        var unitOfWorkFactory = ApplicationLoader.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        var unitOfWork = unitOfWorkFactory.CreateUnitOfWork("Sales");
        var snackBaseRepo = ApplicationLoader.ServiceProvider.GetRequiredService<ISnackRepository>();
        var uuId = Guid.NewGuid();
        var snack = new Snack
                    {
                        UuId = uuId,
                        Name = "Cafe",
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = "System"
                    };
        await snackBaseRepo.AddAsync(snack);
        await unitOfWork.SaveChangesAsync();
        snack.UuId.Should().Be(uuId);
        var snackGet = await snackBaseRepo.FindOneAsync(x => x.UuId == uuId);
        snackGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackGet.ToString());
    }

    [Fact]
    public async Task Should_Add_SnackMachineEntity()
    {
        var unitOfWorkFactory = ApplicationLoader.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        var unitOfWork = unitOfWorkFactory.CreateUnitOfWork("Sales");
        var snackMachineBaseRepo = ApplicationLoader.ServiceProvider.GetRequiredService<ISnackMachineRepository>();
        var uuId = Guid.NewGuid();
        var snackMachine = new SnackMachine
                           {
                               UuId = uuId,
                               Yuan1Inside = 10,
                               Yuan2Inside = 10,
                               Yuan5Inside = 10,
                               Yuan10Inside = 10,
                               Yuan20Inside = 10,
                               Yuan50Inside = 10,
                               Yuan100Inside = 10,
                               CreatedAt = DateTimeOffset.UtcNow,
                               CreatedBy = "System"
                           };
        snackMachine.AmountInside = snackMachine.Yuan1Inside * 1m + snackMachine.Yuan2Inside * 2m + snackMachine.Yuan5Inside * 5m + snackMachine.Yuan10Inside * 10m + snackMachine.Yuan20Inside * 20m + snackMachine.Yuan50Inside * 50m
                                  + snackMachine.Yuan100Inside * 100m;
        await snackMachineBaseRepo.AddAsync(snackMachine);
        await unitOfWork.SaveChangesAsync();
        snackMachine.UuId.Should().Be(uuId);
        var snackMachineGet = await snackMachineBaseRepo.FindOneAsync(x => x.UuId == uuId);
        snackMachineGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackMachineGet.ToString());
    }
}

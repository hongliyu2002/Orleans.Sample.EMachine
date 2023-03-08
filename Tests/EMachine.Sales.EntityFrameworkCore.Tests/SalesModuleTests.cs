using System;
using System.Threading.Tasks;
using EMachine.Sales.Domain.Entities;
using EMachine.Sales.Domain.Repositories;
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
        Initialize();
    }

    protected IUnitOfWork UnitOfWork { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        StopApplication();
    }

    public void Initialize()
    {
        StartApplication();
        var unitOfWorkFactory = ApplicationLoader.ServiceProvider.GetRequiredService<IUnitOfWorkFactory>();
        UnitOfWork = unitOfWorkFactory.CreateUnitOfWork("Sales");
    }

    [Fact]
    public async Task Should_Add_SnackEntity()
    {
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
        await UnitOfWork.SaveChangesAsync();
        snack.UuId.Should().Be(uuId);
        var snackGet = await snackBaseRepo.FindOneAsync(x => x.UuId == uuId);
        snackGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackGet.ToString());
    }

    [Fact]
    public async Task Should_Add_SnackMachineEntity()
    {      
        var snackMachineBaseRepo = ApplicationLoader.ServiceProvider.GetRequiredService<ISnackMachineRepository>();
        var uuId = Guid.NewGuid();
        var snackMachine = new SnackMachine()
                           {
                               UuId = uuId,
                               MoneyInside = new Money(){Yuan50 = 10, Yuan100 = 5},
                               CreatedAt = DateTimeOffset.UtcNow,
                               CreatedBy = "System"
                           };
        await snackMachineBaseRepo.AddAsync(snackMachine);
        await UnitOfWork.SaveChangesAsync();
        snackMachine.UuId.Should().Be(uuId);
        var snackMachineGet = await snackMachineBaseRepo.FindOneAsync(x => x.UuId == uuId);
        snackMachineGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackMachineGet.ToString());

    }
}

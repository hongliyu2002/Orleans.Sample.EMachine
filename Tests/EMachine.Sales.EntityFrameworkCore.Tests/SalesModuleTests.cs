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
    public async Task Should_Add_SnackBase()
    {
        var snackBaseRepo = ApplicationLoader.ServiceProvider.GetRequiredService<ISnackBaseRepository>();
        var snack = new SnackBase
                    {
                        Name = "Cafe",
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = "System"
                    };
        await snackBaseRepo.AddAsync(snack);
        await UnitOfWork.SaveChangesAsync();
        snack.ID.Should().Be(1);
        var snackGet = await snackBaseRepo.GetAsync(snack.ID);
        snackGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackGet.ToString());
    }

    [Fact]
    public async Task Should_Add_SnackMachineBase()
    {      
        var snackMachineBaseRepo = ApplicationLoader.ServiceProvider.GetRequiredService<ISnackMachineBaseRepository>();
        var id = Guid.NewGuid();
        var snackMachine = new SnackMachineBase()
                           {
                               InsideYuan10 = 10,
                               CreatedAt = DateTimeOffset.UtcNow,
                               CreatedBy = "System"
                           };
        await snackMachineBaseRepo.AddAsync(snackMachine);
        await UnitOfWork.SaveChangesAsync();
        snackMachine.ID.Should().Be(id);
        var snackMachineGet = await snackMachineBaseRepo.GetAsync(id);
        snackMachineGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackMachineGet.ToString());

    }
}

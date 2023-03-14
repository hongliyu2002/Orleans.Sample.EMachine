using EMachine.Sales.Orleans.EntityFrameworkCore;
using EMachine.Sales.Orleans.States;
using FluentAssertions;
using Fluxera.Extensions.Hosting.Modules.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace EMachine.Sales.EntityFrameworkCore.Tests;

[Collection("Sales")]
public class SalesModuleTests : StartupModuleTestBase<SalesOrleansEntityFrameworkCoreModule>, IDisposable
{
    private readonly SalesDbContext _dbContext;
    private readonly ITestOutputHelper _output;

    public SalesModuleTests(ITestOutputHelper output)
    {
        _output = output;
        StartApplication();
        _dbContext = ApplicationLoader.ServiceProvider.GetRequiredService<SalesDbContext>();
        // _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Thread.Sleep(2000);
        _dbContext.Dispose();
        StopApplication();
    }

    [Fact]
    public async Task Should_Add_Snack()
    {
        var id = Guid.NewGuid();
        var snack = new Snack(id, "Cafe", "https://bkimg.cdn.bcebos.com/pic/94cad1c8a786c917ef547a52c73d70cf3bc75701?x-bce-process=image/watermark,image_d2F0ZXIvYmFpa2UyMjA=,g_7,xp_5,yp_5")
                    {
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = "System"
                    };
        _dbContext.Snacks.Add(snack);
        await _dbContext.SaveChangesAsync();
        snack.Id.Should().Be(id);
        var snackGet = await _dbContext.Snacks.FindAsync(id);
        snackGet.Should().NotBeNull();
        _output.WriteLine(snackGet?.ToString());
    }

    [Fact]
    public async Task Should_Update_Snack()
    {
        var snack = await _dbContext.Snacks.FirstOrDefaultAsync();
        if (snack != null)
        {
            snack.Name = "Coke";
            snack.LastModifiedAt = DateTimeOffset.UtcNow;
            snack.LastModifiedBy = "Leo";
            var success = await _dbContext.SaveChangesAsync();
            success.Should().BeGreaterThan(0);
            _output.WriteLine(snack.ToString());
        }
    }

    [Fact]
    public async Task Should_Add_And_Update_Snack()
    {
        var id = Guid.NewGuid();
        var snack = new Snack(id, "Cafe", "https://bkimg.cdn.bcebos.com/pic/94cad1c8a786c917ef547a52c73d70cf3bc75701?x-bce-process=image/watermark,image_d2F0ZXIvYmFpa2UyMjA=,g_7,xp_5,yp_5")
                    {
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = "System"
                    };
        _dbContext.Snacks.Add(snack);
        await _dbContext.SaveChangesAsync();
        snack.Id.Should().Be(id);
        var snackForUpdate = await _dbContext.Snacks.FindAsync(id);
        snackForUpdate.Should().NotBeNull();
        if (snackForUpdate != null)
        {
            snackForUpdate.Name = "Coke";
            snackForUpdate.LastModifiedAt = DateTimeOffset.UtcNow;
            snackForUpdate.LastModifiedBy = "Leo";
            var success = await _dbContext.SaveChangesAsync();
            success.Should().BeGreaterThan(0);
            _output.WriteLine(snackForUpdate.ToString());
        }
    }

    [Fact]
    public async Task Should_Add_SnackMachine()
    {
        var id = Guid.NewGuid();
        var snackMachine = new SnackMachine(id, Money.FiftyYuan, 0, new List<Slot>())
                           {
                               CreatedAt = DateTimeOffset.UtcNow,
                               CreatedBy = "System"
                           };
        _dbContext.SnackMachines.Add(snackMachine);
        await _dbContext.SaveChangesAsync();
        var snacks = await _dbContext.Snacks.Take(3).ToListAsync();
        var snack01 = snacks.Skip(0).Take(1).FirstOrDefault();
        var snack02 = snacks.Skip(1).Take(1).FirstOrDefault();
        var snack03 = snacks.Skip(2).Take(1).FirstOrDefault();
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 0
                               });
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 1,
                                   SnackPile = snack01 switch
                                               {
                                                   null => null, _ => new SnackPile(snack01.Id, 20, 3)
                                               }
                               });
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 2,
                                   SnackPile = snack02 switch
                                               {
                                                   null => null, _ => new SnackPile(snack02.Id, 10, 9)
                                               }
                               });
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 3,
                                   SnackPile = snack03 switch
                                               {
                                                   null => null, _ => new SnackPile(snack03.Id, 15, 6)
                                               }
                               });
        await _dbContext.SaveChangesAsync();
        snackMachine.Id.Should().Be(id);
        var snackMachineGet = await _dbContext.SnackMachines.Include(x => x.Slots).FirstOrDefaultAsync();
        snackMachineGet.Should().NotBeNull();
        snackMachineGet!.Slots.Should().HaveCount(4);
        _output.WriteLine(snackMachineGet.ToString());
    }
}

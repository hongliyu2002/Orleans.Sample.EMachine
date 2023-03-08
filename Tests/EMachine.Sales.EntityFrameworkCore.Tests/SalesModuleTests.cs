using EMachine.Sales.Domain;
using EMachine.Sales.EntityFrameworkCore.Contexts;
using FluentAssertions;
using Fluxera.Extensions.Hosting.Modules.UnitTesting;
using Microsoft.EntityFrameworkCore;
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
        // dbContext.Database.EnsureDeleted();
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
        var dbContext = ApplicationLoader.ServiceProvider.GetRequiredService<SalesDbContext>();
        var id = Guid.NewGuid();
        var snack = new Snack
                    {
                        Id = id,
                        Name = "Cafe",
                        CreatedAt = DateTimeOffset.UtcNow,
                        CreatedBy = "System"
                    };
        await dbContext.Snacks.AddAsync(snack);
        await dbContext.SaveChangesAsync();
        snack.Id.Should().Be(id);
        var snackGet = await dbContext.Snacks.FindAsync(id);
        snackGet.Should().NotBeNull();
        _testOutputHelper.WriteLine(snackGet!.ToString());
    }

    [Fact]
    public async Task Should_Add_SnackMachineEntity()
    {
        var dbContext = ApplicationLoader.ServiceProvider.GetRequiredService<SalesDbContext>();
        var id = Guid.NewGuid();
        var snackMachine = new SnackMachine
                           {
                               Id = id,
                               MoneyInside = new Money
                                             {
                                                 Yuan1 = 10,
                                                 Yuan2 = 9,
                                                 Yuan5 = 8,
                                                 Yuan10 = 7,
                                                 Yuan20 = 6,
                                                 Yuan50 = 5,
                                                 Yuan100 = 4
                                             },
                               CreatedAt = DateTimeOffset.UtcNow,
                               CreatedBy = "System"
                           };
        snackMachine.MoneyInside.Amount = snackMachine.MoneyInside.Yuan1 * 1m + snackMachine.MoneyInside.Yuan2 * 2m + snackMachine.MoneyInside.Yuan5 * 5m + snackMachine.MoneyInside.Yuan10 * 10m + snackMachine.MoneyInside.Yuan20 * 20m
                                        + snackMachine.MoneyInside.Yuan50 * 50m + snackMachine.MoneyInside.Yuan100 * 100m;
        await dbContext.SnackMachines.AddAsync(snackMachine);
        await dbContext.SaveChangesAsync();
        var snacks = await dbContext.Snacks.Take(3).ToListAsync();
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
                                                   null => null,
                                                   _ => new SnackPile
                                                        {
                                                            SnackId = snack01.Id,
                                                            Quantity = 20,
                                                            Price = 3
                                                        }
                                               }
                               });
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 2,
                                   SnackPile = snack02 switch
                                               {
                                                   null => null,
                                                   _ => new SnackPile
                                                        {
                                                            SnackId = snack02.Id,
                                                            Quantity = 10,
                                                            Price = 9
                                                        }
                                               }
                               });
        snackMachine.Slots.Add(new Slot
                               {
                                   MachineId = id,
                                   Position = 3,
                                   SnackPile = snack03 switch
                                               {
                                                   null => null,
                                                   _ => new SnackPile
                                                        {
                                                            SnackId = snack03.Id,
                                                            Quantity = 15,
                                                            Price = 6
                                                        }
                                               }
                               });
        await dbContext.SaveChangesAsync();
        snackMachine.Id.Should().Be(id);
        var snackMachineGet = await dbContext.SnackMachines.Include(x => x.Slots).FirstOrDefaultAsync();
        snackMachineGet.Should().NotBeNull();
        snackMachineGet.Slots.Should().HaveCount(4);
        _testOutputHelper.WriteLine(snackMachineGet!.ToString());
    }
}

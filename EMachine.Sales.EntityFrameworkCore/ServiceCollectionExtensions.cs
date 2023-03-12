using EMachine.Sales.EntityFrameworkCore.Contexts;
using Fluxera.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EMachine.Sales.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextPoolForSales(this IServiceCollection services, EfCoreDatabaseOptions options)
    {
        return services.AddDbContextPool<SalesDbContext>(dbContext =>
                                                         {
                                                             if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
                                                             {
                                                                 switch (options.DatabaseProvider)
                                                                 {
                                                                     case EfCoreDatabaseProvider.Sqlite:
                                                                         UseSqlite(dbContext, connectionString, options);
                                                                         break;
                                                                     case EfCoreDatabaseProvider.SqlServer:
                                                                         UseSqlServer(dbContext, connectionString, options);
                                                                         break;
                                                                     case EfCoreDatabaseProvider.PostgreSql:
                                                                         break;
                                                                     case EfCoreDatabaseProvider.MySql:
                                                                         break;
                                                                     default:
                                                                         throw new ArgumentOutOfRangeException(nameof(options.DatabaseProvider), "Database provider does not support.");
                                                                 }
                                                             }
                                                         });
    }

    private static void UseSqlite(DbContextOptionsBuilder dbContextBuilder, string connectionString, EfCoreDatabaseOptions databaseOptions)
    {
        dbContextBuilder.UseSqlite(connectionString, options =>
                                                     {
                                                         if (databaseOptions.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                         {
                                                             var schemaAndName = databaseOptions.MigrationsHistoryTable!.Trim().Split(".");
                                                             if (schemaAndName.Length >= 1)
                                                             {
                                                                 options.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                             }
                                                             else
                                                             {
                                                                 options.MigrationsHistoryTable(schemaAndName[0]);
                                                             }
                                                         }
                                                         options.UseQuerySplittingBehavior(databaseOptions.QuerySplittingBehavior);
                                                     });
    }

    private static void UseSqlServer(DbContextOptionsBuilder dbContextBuilder, string connectionString, EfCoreDatabaseOptions efOptions)
    {
        dbContextBuilder.UseSqlServer(connectionString, options =>
                                                        {
                                                            if (efOptions.MigrationsHistoryTable.IsNotNullOrEmpty())
                                                            {
                                                                var schemaAndName = efOptions.MigrationsHistoryTable!.Trim().Split(".");
                                                                if (schemaAndName.Length >= 1)
                                                                {
                                                                    options.MigrationsHistoryTable(schemaAndName[1], schemaAndName[0]);
                                                                }
                                                                else
                                                                {
                                                                    options.MigrationsHistoryTable(schemaAndName[0]);
                                                                }
                                                            }
                                                            options.EnableRetryOnFailure(efOptions.MaxRetry, TimeSpan.FromMilliseconds(efOptions.MaxRetryDelay), new[] { -1000 });
                                                            options.UseQuerySplittingBehavior(efOptions.QuerySplittingBehavior);
                                                        });
    }
}

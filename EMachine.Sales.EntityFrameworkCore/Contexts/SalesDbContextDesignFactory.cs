using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EMachine.Sales.EntityFrameworkCore.Contexts;

public class SalesDbContextDesignFactory : IDesignTimeDbContextFactory<SalesDbContext>
{

    /// <inheritdoc />
    public SalesDbContext CreateDbContext(string[] args)
    {
        // var connectionString = "Data Source=Sales.db";
        // var builder = new DbContextOptionsBuilder<SalesDbContext>().UseSqlite(connectionString);
        var connectionString = "Server=123.60.184.85;Integrated Security=False;User Id=sa;Password=Bosshong2010;TrustServerCertificate=True;Database=Sales";
        var builder = new DbContextOptionsBuilder<SalesDbContext>().UseSqlServer(connectionString);
        return new SalesDbContext(builder.Options);
    }
}

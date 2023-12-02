using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Publisher;

public class PublisherDbContextFactory : IDesignTimeDbContextFactory<PublisherDbContext>
{
    public PublisherDbContext CreateDbContext(string[] args)
    {
        // Get environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Publisher"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<PublisherDbContext>();

        optionsBuilder.UseNpgsql(config.GetConnectionString("postgreSqlConnection"),
                b => b.MigrationsAssembly("Repository"));

        return new PublisherDbContext(optionsBuilder.Options);
    }
}
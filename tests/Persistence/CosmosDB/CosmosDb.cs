using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Household.Tests.Persistence.Fixtures;

namespace CosmosDB;

public class ExploreCosmosDb
{
    [Test]
    public async Task Play()
    {
        var services = new ServiceCollection();

        // Configure EF Core with Cosmos DB
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseCosmos(
                connectionString: "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "TestDatabase"
            );
        });

        // Build service provider
        var serviceProvider = services.BuildServiceProvider();

        // Get the context and ensure database is created
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure database and container are created (equivalent to migrations for Cosmos DB)
        await context.Database.EnsureCreatedAsync();

        // Your test context is now ready for test cases
        Assert.That(context, Is.Not.Null);
    }
}

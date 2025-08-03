using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Household.Tests.Persistence.Fixtures;

namespace SQLite;

public class SQLite
{
    [Test]
    public async Task DbProvider_Test()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite("DataSource=:memory:"); // Use in-memory SQLite for testing
        });
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        Assert.That(context, Is.Not.Null);
    }
}

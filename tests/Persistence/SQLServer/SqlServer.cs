using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Household.Tests.Persistence.Fixtures;

namespace SQLServer;

public class SqlServer
{
    [Test]
    public async Task DbProvider_Test()
    {
        var services = new ServiceCollection();

        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer("Server=localhost;Database=TestDatabase;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"); // Use SQL Server for testing
        });
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        Assert.That(context, Is.Not.Null);
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Traqtiv.API.Data;

namespace Traqtiv.Mobile.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registrations
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<SmartFitnessDb>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(SmartFitnessDb) ||
                d.ServiceType.FullName?.Contains("IDbContextOptionsConfiguration") == true ||
                (d.ServiceType.IsGenericType &&
                 d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextOptionsConfiguration<>))
            ).ToList();

            foreach (var descriptor in descriptors)
                services.Remove(descriptor);

            // Add InMemory Database
            services.AddDbContext<SmartFitnessDb>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Create-DB
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SmartFitnessDb>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }
}
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Infrastructure.Data
{
    public class WebApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<WebApplicationDbContext>
    {
        public WebApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .AddUserSecrets<WebApplicationDbContextDesignTimeFactory>(true)
                    .Build();
            var options =
                new DbContextOptionsBuilder<WebApplicationDbContext>()
                    .UseSqlServer(configuration.GetConnectionString("WebApplication"))
                    .Options;
            return new WebApplicationDbContext(options);
        }
    }
}
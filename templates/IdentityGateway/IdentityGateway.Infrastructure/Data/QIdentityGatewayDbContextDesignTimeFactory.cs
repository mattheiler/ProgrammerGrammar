using System.IO;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IdentityGateway.Infrastructure.Data
{
    public class IdentityGatewayDbContextDesignTimeFactory : IDesignTimeDbContextFactory<IdentityGatewayDbContext>
    {
        public IdentityGatewayDbContext CreateDbContext(string[] args)
        {
            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .AddUserSecrets<IdentityGatewayDbContextDesignTimeFactory>(true)
                    .Build();
            var options =
                new DbContextOptionsBuilder<IdentityGatewayDbContext>()
                    .UseSqlServer(configuration.GetConnectionString("IdentityGateway"))
                    .Options;
            return new IdentityGatewayDbContext(options, Options.Create(new OperationalStoreOptions()));
        }
    }
}
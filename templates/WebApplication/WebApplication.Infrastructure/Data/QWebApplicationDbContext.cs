using Microsoft.EntityFrameworkCore;
using WebApplication.Core.Abstractions;

namespace WebApplication.Infrastructure.Data
{
    public class WebApplicationDbContext : DbContext, IWebApplicationDbContext
    {
        public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
        }
    }
}
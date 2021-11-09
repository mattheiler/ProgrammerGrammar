using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Core.Abstractions
{
    public interface IWebApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
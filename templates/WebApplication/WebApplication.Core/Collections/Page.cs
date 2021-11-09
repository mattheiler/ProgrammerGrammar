using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Core.Collections
{
    public static class Page
    {
        public static async Task<Page<T>> ToPageAsync<T>(this IQueryable<T> @this, int offset, int limit, CancellationToken cancellationToken)
        {
            var count = await @this.CountAsync(cancellationToken);
            var items = await @this.Skip(offset).Take(limit).ToListAsync(cancellationToken);
            return new Page<T>(items, count);
        }
    }

    public class Page<T>
    {
        public Page(IEnumerable<T> items, int count)
        {
            Count = count;
            Items = items.ToList().AsReadOnly();
        }

        public int Count { get; }

        public IReadOnlyList<T> Items { get; }
    }
}
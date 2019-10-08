using System.Threading.Tasks;
using Czeum.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Czeum.DAL.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<TSource> CustomFindAsync<TSource>(
            this DbSet<TSource> dbSet, 
            object key,
            string? message = null)
            where TSource : class
        {
            return await dbSet.FindAsync(key) ?? throw new NotFoundException(message);
        }
    }
}

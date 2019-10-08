using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Czeum.Core.Exceptions;

namespace Czeum.DAL.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<TSource> CustomSingleAsync<TSource>(
            this IQueryable<TSource> source, 
            Expression<Func<TSource, bool>> predicate, 
            string? message = null)
            where TSource : class
        {
            return await source.SingleOrDefaultAsync(predicate) ?? throw new NotFoundException(message);
        }
    }
}

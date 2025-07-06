using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Autovelox.Application.Extensions
{
    public static class LinqExtensions
    {
        // Per IEnumerable<T> (es. liste in memoria)
        public static IEnumerable<T> WhereIf<T>(
            this IEnumerable<T> source,
            bool condition,
            Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        // Per IQueryable<T> (es. LINQ to SQL, Entity Framework)
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> source,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }

}

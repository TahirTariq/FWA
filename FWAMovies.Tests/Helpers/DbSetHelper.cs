using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FWAMovies.Tests.Helpers
{
    public class DbSetHelper
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(IList<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}

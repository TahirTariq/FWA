using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace FWAMovies.DAL.Interface
{
    public interface IDbContext
    {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;       
        Task<int> SaveChangesAsync();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
    }
}

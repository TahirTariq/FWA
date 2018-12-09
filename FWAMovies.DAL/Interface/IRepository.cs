using FWAMovies.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FWAMovies.DAL.Interface
{
    
    /// <summary>
    /// This a slimed down version of a Generic repository 
    /// that I have started developing recently
    /// </summary>
    /// <typeparam name="TEntity">EF Entity</typeparam>
    public interface IRepository<TEntity>
       where TEntity : class, IEntity
    {
        object Context();

        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null);

        TEntity GetById(object id);
            
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entity);

        void Save();

        Task SaveAsync();
    }
}

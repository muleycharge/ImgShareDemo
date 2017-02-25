namespace ImgShareDemo.DAL
{
    using ImgShareDemo.BO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        DbSet<TEntity> GetDbSet();

        Task<DbSet<TEntity>> GetDbSetAsync();

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties);
        
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties);

        Task<TEntity> GetByIdAsync(int id,
            params string[] includeProperties);

        TEntity GetById(int id,
            params string[] includeProperties);
        void InsertOrUpdate(TEntity entity);

        void Insert(TEntity entity);

        Task DeleteAsync(int id);

        void Delete(int id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        Task UpdateAsync(TEntity entityToUpdate);
    }
}

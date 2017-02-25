#pragma warning disable 1587
/// <summary>
/// This generic repository uses a variation of the repository pattern. The steps to add a new entity are as follows:
/// 1. Define the entity class along with its code first mappings in ImgShareDemo.BO, make sure it implements
///    the IEntity interface.
/// 2. Create an interface for the repository in ImgShareDemo.DAL.Repositories that implements IGenericRepository
/// 3. Create an implementation for the repository in ImgShareDemo.DAL.Repositories that implements the
///    interface created in step 2 and inherits from GenericRepository. Typically this class is empty,
///    but the purpose of it is to contain any custom data access methods pertaining to your new entity.
/// 4. Add DbSet property for your new entity in DbContext.cs, along with any custom table definitions
///    in the OnModelCreating method.
/// 5. Add a Lazy accessor for your new repository to UnitOfWork.cs
/// </summary>
#pragma warning restore 1587
namespace ImgShareDemo.DAL
{
    using ImgShareDemo.BO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Base repository class implementing standard set of DAL methods on Entity objects.
    /// All non-generic repository interfaces inherit from IGenericRepository and in turn
    /// all classes that implement them. This allows all non-generic implementations of 
    /// IGenericRepository to use common implementations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        #region Fields
        internal ImgShareDemoContext context;
        internal DbSet<TEntity> dbSet;
        #endregion

        #region Constructors
        public GenericRepository(ImgShareDemoContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Methods
        public DbSet<TEntity> GetDbSet()
        {
            return dbSet;
        }

        public async Task<DbSet<TEntity>> GetDbSetAsync()
        {
            return await Task.Run(() =>
            {
                return GetDbSet();
            });
        }

        public virtual IQueryable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IList<Expression<Func<TEntity, bool>>> filters = new List<Expression<Func<TEntity, bool>>>();
            if (filter != null)
            {
                filters.Add(filter);
            }

            return GetQuery(filters, orderBy, includeProperties);
        }

        public IQueryable<TEntity> GetQuery(
            IList<Expression<Func<TEntity, bool>>> filters,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;
            query.Include(t => t.Id);

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            return await GetQuery(filter, orderBy, includeProperties).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            IList<Expression<Func<TEntity, bool>>> filters,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            return await GetQuery(filters, orderBy, includeProperties).ToListAsync().ConfigureAwait(false);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties)
        {
            return GetQuery(filter, orderBy, includeProperties).ToList();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id,
            params string[] includeProperties)
        {
            DbQuery<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties ?? new string[0])
            {
                query = query.Include(includeProperty);
            }
            return await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public virtual TEntity GetById(int id,
            params string[] includeProperties)
        {
            DbQuery<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties ?? new string[0])
            {
                query = query.Include(includeProperty);
            }
            return query.SingleOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Inserts the passed in entity
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void InsertOrUpdate(TEntity entity)
        {
            if (entity.Id == 0)
            {
                Insert(entity);
            }
            else
            {
                Update(entity);
            }
        }

        /// <summary>
        /// Inserts the passed in entity
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            if (entity is ITrackable)
            {
                ITrackable trackable = entity as ITrackable;
                trackable.DateCreated = DateTime.UtcNow;
            }
            dbSet.Add(entity);
        }

        /// <summary>
        /// Deletes the entity associated with the passed in ID.
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(int id)
        {
            TEntity entityToDelete = await GetByIdAsync(id).ConfigureAwait(false);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the entity associated with the passed in ID.
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(int id)
        {
            TEntity entityToDelete = GetById(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the passed in entity
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Updates entity model with values from the passed in entity.
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate is ITrackable)
            {
                ITrackable trackable = entityToUpdate as ITrackable;
                trackable.DateModified = DateTime.UtcNow;
            }

            dbSet.Attach(entityToUpdate);

            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Updates entity model with values from the passed in entity.
        /// Note: SaveChanges must be called for changes to be sent to
        /// the database.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual async Task UpdateAsync(TEntity entityToUpdate)
        {
            await Task.Run(() => { Update(entityToUpdate); }).ConfigureAwait(false);
        }
        #endregion
    }
}

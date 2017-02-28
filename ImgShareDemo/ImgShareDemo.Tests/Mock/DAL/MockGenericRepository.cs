namespace ImgShareDemo.Tests.Mock.DAL
{
    using BO;
    using ImgShareDemo.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq.Expressions;

    public abstract class MockGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        protected Dictionary<int, TEntity> _mockDb;

        public MockGenericRepository(Dictionary<int, TEntity> mockDb)
        {
            _mockDb = mockDb;
        }

        public void Delete(TEntity entityToDelete)
        {
            _mockDb.Remove(entityToDelete.Id);
        }

        public void Delete(int id)
        {
            _mockDb.Remove(id);
        }

        public Task DeleteAsync(int id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            var result = _mockDb.Values.AsQueryable().Where(filter);
            if(orderBy != null)
            {
                result = orderBy(result);
            }
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            var es = Get(filter, orderBy);
            await Task.CompletedTask.ConfigureAwait(false);
            return es;
        }

        public TEntity GetById(int id, params string[] includeProperties)
        {
            return _mockDb[id];
        }

        public async Task<TEntity> GetByIdAsync(int id, params string[] includeProperties)
        {
            TEntity e = GetById(id);
            await Task.CompletedTask.ConfigureAwait(false);
            return e;
        }

        public void Insert(TEntity entity)
        {
            entity.Id = _mockDb.Keys.Any() ? _mockDb.Keys.Max() + 1 : 1;
            _mockDb.Add(entity.Id, entity);
        }

        public void InsertOrUpdate(TEntity entity)
        {
            if(_mockDb.ContainsKey(entity.Id))
            {
                _mockDb[entity.Id] = entity;
            }
            else
            {
                Insert(entity);
            }
        }

        public void Update(TEntity entityToUpdate)
        {
            if (_mockDb.ContainsKey(entityToUpdate.Id))
            {
                _mockDb[entityToUpdate.Id] = entityToUpdate;
                return;
            }
            throw new InvalidOperationException($"Entity with id {entityToUpdate.Id} does not exist.");
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            Update(entityToUpdate);
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}

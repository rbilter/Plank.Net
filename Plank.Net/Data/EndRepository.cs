using Plank.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public sealed class EndRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region PROPERTIES

        public IRepository<TEntity> NextRepository { get; }

        #endregion

        #region METHODS

        public async Task AddAsync(TEntity _)
        {
            await Task.Yield();
        }

        public async Task BulkAddAsync(IEnumerable<TEntity> _)
        {
            await Task.Yield();
        }

        public async Task DeleteAsync(int _)
        {
            await Task.Yield();
        }

        public Task<TEntity> GetAsync(int _)
        {
            return Task.FromResult<TEntity>(null);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> _)
        {
            return null;
        }

        public Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> _, List<Expression<Func<TEntity, object>>> includes, int pageNumber, int pageSize)
        {
            return Task.FromResult<IPagedList<TEntity>>(null);
        }

        public async Task UpdateAsync(TEntity _)
        {
            await Task.Yield();
        }

        #endregion
    }
}

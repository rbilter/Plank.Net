using Plank.Net.Models;
using System;
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

        public async Task AddAsync(TEntity entity)
        {
            await Task.Yield();
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Yield();
        }

        public Task<TEntity> GetAsync(int id)
        {
            return Task.FromResult<TEntity>(null);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> repository)
        {
            return null;
        }

        public Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            return Task.FromResult<IPagedList<TEntity>>(null);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Yield();
        }

        #endregion
    }
}

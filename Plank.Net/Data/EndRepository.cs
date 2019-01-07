using PagedList;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public sealed class EndRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region METHODS

        public override async Task<int> CreateAsync(TEntity entity)
        {
            return await Task.Run(() => entity.Id);
        }

        public override async Task<int> DeleteAsync(int id)
        {
            return await Task.Run(() => id);
        }

        public override Task<TEntity> GetAsync(int id)
        {
            return Task.FromResult<TEntity>(null);
        }

        public override Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            return Task.FromResult<IPagedList<TEntity>>(null);
        }

        public override async Task<int> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() => entity.Id);
        }

        public override async Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return await Task.Run(() => entity.Id);
        }

        #endregion
    }
}

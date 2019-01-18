﻿using PagedList;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public sealed class EndRepository<TEntity> : AbstractRepository<TEntity> where TEntity : class, IEntity
    {
        #region METHODS

        public override async Task CreateAsync(TEntity entity)
        {
            await Task.Yield();
        }

        public override async Task DeleteAsync(int id)
        {
            await Task.Yield();
        }

        public override Task<TEntity> GetAsync(int id)
        {
            return Task.FromResult<TEntity>(null);
        }

        public override Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            return Task.FromResult<IPagedList<TEntity>>(null);
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            await Task.Yield();
        }

        public override async Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            await Task.Yield();
        }

        #endregion
    }
}

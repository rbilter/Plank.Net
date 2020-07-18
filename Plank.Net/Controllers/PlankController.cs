using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Models;
using Plank.Net.Search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Controllers
{
    public sealed class PlankController<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly PlankManager<TEntity> _manager;

        #endregion

        #region  CONSTRUCTORS

        public PlankController(DbContext context)
        {
            var repo = new PlankRepository<TEntity>(context);
            repo.RegisterNext(new EndRepository<TEntity>());

            var logger = new PlankLogger<TEntity>();
            _manager = new PlankManager<TEntity>(repo, logger);
        }

        #endregion

        #region METHODS

        public async Task<PlankPostResponse<TEntity>> AddAsync(TEntity entity)
        {
            return await _manager.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task<PlankBulkPostResponse<TEntity>> BulkAddAsync(IEnumerable<TEntity> entities)
        {
            return await _manager.BulkAddAsync(entities).ConfigureAwait(false);
        }

        public async Task<PlankDeleteResponse> DeleteAsync(int id)
        {
            return await _manager.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<PlankGetResponse<TEntity>> GetAsync(int id)
        {
            return await _manager.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<PlankEnumerableResponse<TEntity>> SearchAsync(ISearchBuilder<TEntity> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            var expression = builder.Build();
            var includes   = builder.Includes ?? new List<Expression<Func<TEntity, object>>>();
            var pageNumber = builder.PageNumber;
            var pageSize   = builder.PageSize;

            return await _manager.SearchAsync(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            return await _manager.UpdateAsync(entity).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return await _manager.UpdateAsync(entity, properties).ConfigureAwait(false);
        }

        #endregion
    }
}

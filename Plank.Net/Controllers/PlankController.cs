using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Models;
using Plank.Net.Search;
using System;
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
            return await _manager.AddAsync(entity);
        }

        public async Task<PlankDeleteResponse> DeleteAsync(int id)
        {
            return await _manager.DeleteAsync(id);
        }

        public async Task<PlankGetResponse<TEntity>> GetAsync(int id)
        {
            return await _manager.GetAsync(id);
        }

        public async Task<PlankEnumerableResponse<TEntity>> SearchAsync(ISearchBuilder<TEntity> builder)
        {
            var expression = builder.Build();
            var pageNumber = builder.PageNumber;
            var pageSize   = builder.PageSize;

            return await _manager.SearchAsync(expression, pageNumber, pageSize);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            return await _manager.UpdateAsync(entity);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return await _manager.UpdateAsync(entity, properties);
        }

        #endregion
    }
}

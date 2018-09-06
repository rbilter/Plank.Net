using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Profiles;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Controllers
{
    public sealed class EntityController<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly EntityManager<TEntity> _manager;

        #endregion

        #region  CONSTRUCTORS

        public EntityController(DbContext context)
        {
            var repo   = new EntityRepository<TEntity>(context);
            var logger = new EntityLogger<TEntity>();

            _manager    = new EntityManager<TEntity>(repo, logger);
        }

        #endregion

        #region METHODS

        public async Task<ApiPostResponse> CreateAsync(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(await _manager.CreateAsync(entity));
        }

        public async Task<ApiPostResponse> DeleteAsync(int id)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(await _manager.DeleteAsync(id));
        }

        public async Task<ApiGetResponse<TEntity>> GetAsync(int id)
        {
            return Mapping<TEntity>.Mapper.Map<ApiGetResponse<TEntity>>(await _manager.GetAsync(id));
        }

        public async Task<ApiEnumerableResponse<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            return Mapping<TEntity>.Mapper.Map<ApiEnumerableResponse<TEntity>>(await _manager.SearchAsync(expression, pageNumber, pageSize));
        }

        public async Task<ApiPostResponse> UpdateAsync(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(await _manager.UpdateAsync(entity));
        }

        public async Task<ApiPostResponse> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(await _manager.UpdateAsync(entity, properties));
        }

        #endregion
    }
}

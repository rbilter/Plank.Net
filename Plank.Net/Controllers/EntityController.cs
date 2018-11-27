using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Profiles;
using Plank.Net.Search;
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

        public async Task<PlankPostResponse> CreateAsync(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<PlankPostResponse>(await _manager.CreateAsync(entity));
        }

        public async Task<PlankPostResponse> DeleteAsync(int id)
        {
            return Mapping<TEntity>.Mapper.Map<PlankPostResponse>(await _manager.DeleteAsync(id));
        }

        public async Task<PlankGetResponse<TEntity>> GetAsync(int id)
        {
            return Mapping<TEntity>.Mapper.Map<PlankGetResponse<TEntity>>(await _manager.GetAsync(id));
        }

        public async Task<PlankEnumerableResponse<TEntity>> SearchAsync(ISearchBuilder<TEntity> builder)
        {
            builder.Build();
            return Mapping<TEntity>.Mapper.Map<PlankEnumerableResponse<TEntity>>(await _manager.SearchAsync(builder.SearchExpression, builder.PageNumber, builder.PageSize));
        }

        public async Task<PlankPostResponse> UpdateAsync(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<PlankPostResponse>(await _manager.UpdateAsync(entity));
        }

        public async Task<PlankPostResponse> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return Mapping<TEntity>.Mapper.Map<PlankPostResponse>(await _manager.UpdateAsync(entity, properties));
        }

        #endregion
    }
}

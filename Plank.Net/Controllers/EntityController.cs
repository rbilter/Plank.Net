using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Profiles;
using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Plank.Net.Controllers
{
    public sealed class EntityController<TEntity> where TEntity : Entity
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

        public ApiPostResponse Create(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(_manager.Create(entity));
        }

        public ApiPostResponse Delete(Guid id)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(_manager.Delete(id));
        }

        public ApiGetResponse<TEntity> Get(Guid id)
        {
            return Mapping<TEntity>.Mapper.Map<ApiGetResponse<TEntity>>(_manager.Get(id));
        }

        public ApiEnumerableResponse<TEntity> Search(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            return Mapping<TEntity>.Mapper.Map<ApiEnumerableResponse<TEntity>>(_manager.Search(expression, pageNumber, pageSize));
        }

        public ApiPostResponse Update(TEntity entity)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(_manager.Update(entity));
        }

        public ApiPostResponse Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return Mapping<TEntity>.Mapper.Map<ApiPostResponse>(_manager.Update(entity, properties));
        }

        #endregion
    }
}

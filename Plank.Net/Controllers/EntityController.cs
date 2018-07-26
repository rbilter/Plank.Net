using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Profiles;
using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Plank.Net.Controllers
{
    public sealed class EntityController<T> where T : Entity
    {
        #region MEMBERS

        private readonly EntityManager<T> _manager;

        #endregion

        #region  CONSTRUCTORS

        public EntityController(DbContext context)
        {
            var repo   = new EntityRepository<T>(context);
            var logger = new EntityLogger<T>();

            _manager    = new EntityManager<T>(repo, logger);
        }

        #endregion

        #region METHODS

        public ApiPostResponse Create(T entity)
        {
            return Mapping<T>.Mapper.Map<ApiPostResponse>(_manager.Create(entity));
        }

        public ApiPostResponse Delete(Guid id)
        {
            return Mapping<T>.Mapper.Map<ApiPostResponse>(_manager.Delete(id));
        }

        public ApiGetResponse<T> Get(Guid id)
        {
            return Mapping<T>.Mapper.Map<ApiGetResponse<T>>(_manager.Get(id));
        }

        public ApiEnumerableResponse<T> Search(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            return Mapping<T>.Mapper.Map<ApiEnumerableResponse<T>>(_manager.Search(expression, pageNumber, pageSize));
        }

        public ApiPostResponse Update(T entity)
        {
            return Mapping<T>.Mapper.Map<ApiPostResponse>(_manager.Update(entity));
        }

        public ApiPostResponse Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            return Mapping<T>.Mapper.Map<ApiPostResponse>(_manager.Update(entity, properties));
        }

        #endregion
    }
}

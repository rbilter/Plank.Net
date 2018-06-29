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

        public PlankPostResponse Create(T entity)
        {
            return Mapping<T>.Mapper.Map<PlankPostResponse>(_manager.Create(entity));
        }

        public PlankPostResponse Delete(Guid id)
        {
            return Mapping<T>.Mapper.Map<PlankPostResponse>(_manager.Delete(id));
        }

        public PlankGetResponse<T> Get(Guid id)
        {
            return Mapping<T>.Mapper.Map<PlankGetResponse<T>>(_manager.Get(id));
        }

        public PlankEnumerationResponse<T> Search(T entity)
        {
            return Mapping<T>.Mapper.Map<PlankEnumerationResponse<T>>(_manager.Search(entity));
        }

        public PlankPostResponse Update(T entity)
        {
            return Mapping<T>.Mapper.Map<PlankPostResponse>(_manager.Update(entity));
        }

        public PostResponse Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            return Mapping<T>.Mapper.Map<PostResponse>(_manager.Update(entity, properties));
        }

        #endregion
    }
}

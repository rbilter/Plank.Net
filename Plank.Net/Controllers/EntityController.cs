using Plank.Net.Data;
using Plank.Net.Managers;
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

        public PostResponse Create(T entity)
        {
            return _manager.Create(entity);
        }

        public PostResponse Delete(Guid id)
        {
            return _manager.Delete(id);
        }

        public GetResponse<T> Get(Guid id)
        {
            return _manager.Get(id);
        }

        public PostEnumerationResponse<T> Search(T entity)
        {
            return _manager.Search(entity);
        }

        public PostResponse Update(T entity)
        {
            return _manager.Update(entity);
        }

        public PostResponse Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            return _manager.Update(entity, properties);
        }

        #endregion
    }
}

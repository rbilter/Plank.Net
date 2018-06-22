using Plank.Net.Data;
using Plank.Net.Managers;
using System;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Description;

namespace Plank.Net.Controllers
{
    public class EntityController<T> : ApiController where T : Entity
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

        [HttpPost]
        [ResponseType(typeof(PostResponse))]
        public PostResponse Create(T entity)
        {
            return _manager.Create(entity);
        }

        [HttpPost]
        [ResponseType(typeof(PostResponse))]
        public PostResponse Delete(Guid id)
        {
            return _manager.Delete(id);
        }

        [HttpGet]
        [ResponseType(typeof(GetResponse<>))]
        public GetResponse<T> Get(Guid id)
        {
            return _manager.Get(id);
        }

        [HttpPost]
        [ResponseType(typeof(PostEnumerationResponse<>))]
        public PostEnumerationResponse<T> Search(T entity)
        {
            return _manager.Search(entity);
        }

        [HttpPost]
        [ResponseType(typeof(PostResponse))]
        public PostResponse Update(T entity)
        {
            return _manager.Update(entity);
        }

        #endregion
    }
}

using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;
using Plank.Net.Profiles;
using Plank.Net.Utilities;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Plank.Net.Managers
{
    public sealed class EntityManager<TEntity> : IEntityManager<TEntity> where TEntity : Entity
    {
        #region MEMBERS

        private readonly IEntityRepository<TEntity> _repository;
        private readonly ILogger<TEntity> _logger;

        #endregion

        #region CONSTRUCTORS

        public EntityManager(DbContext context)
            : this(new EntityRepository<TEntity>(context), new EntityLogger<TEntity>())
        {

        }

        public EntityManager(IEntityRepository<TEntity> repository, ILogger<TEntity> logger)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
            _logger     = logger ?? throw new ArgumentNullException("logger");
        }

        #endregion

        #region METHODS

        public PostResponse Create(TEntity entity)
        {
            _logger.Info(entity.ToJson());

            var newid      = 0;
            var validation = ValidateEntity(entity);

            if (validation.IsValid)
            {
                try
                {
                    newid = _repository.Create(entity);
                }
                catch (DataException e)
                {
                    _logger.Error(e);

                    var msg = "There was an issue processing the request, please try again";
                    var valresult = new ValidationResult(msg, this, "Error", null, null);

                    validation.AddResult(valresult);
                }
            }

            var results = new PostResponse { Id = newid, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        public PostResponse Delete(int id)
        {
            _logger.Info(id);

            var validation = new ValidationResults();

            try
            {
                if (_repository.Get(id) != null)
                {
                    _repository.Delete(id);
                }
            }
            catch (DataException e)
            {
                _logger.Error(e);

                var msg = "There was an issue processing the request, please try again";
                var valresult = new ValidationResult(msg, this, "Error", null, null);

                validation.AddResult(valresult);
            }

            var results = new PostResponse { Id = id, ValidationResults = validation };

            _logger.Info(results.ToJson());
            return results;
        }

        public GetResponse<TEntity> Get(int id)
        {
            _logger.Info(id);
            GetResponse<TEntity> result = null;

            try
            {
                var item = _repository.Get(id);
                result = new GetResponse<TEntity>(item);
                result.IsValid = true;
            }
            catch (DataException e)
            {
                _logger.Error(e);
                result = new GetResponse<TEntity>();
                result.IsValid = false;
                result.Message = "There was an issue processing the request, please try again";
            }

            _logger.Info(result.ToJson());
            return result;
        }

        public PostEnumerableResponse<TEntity> Search(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            _logger.Info(expression);

            PostEnumerableResponse<TEntity> result = null;
            expression = expression ?? (f => true);

            try
            {
                var pagedList  = _repository.Search(expression, pageNumber, pageSize);
                result         = Mapping<TEntity>.Mapper.Map<PostEnumerableResponse<TEntity>>(pagedList);
                result.IsValid = true;
            }
            catch(DataException e)
            {
                _logger.Error(e);
                result         = new PostEnumerableResponse<TEntity>();
                result.IsValid = false;
                result.Message = "There was an issue processing the request, please try again";
            }

            _logger.Info(result.ToJson());
            return result;
        }

        public PostResponse Update(TEntity entity)
        {
            _logger.Info(entity.ToJson());

            var id         = 0;
            var validation = ValidateEntity(entity);

            if (validation.IsValid)
            {
                try
                {
                    if (_repository.Get(entity.Id) != null)
                    {
                        id = _repository.Update(entity);
                    }
                    else
                    {
                        var msg = "Item could not be found.";
                        var valresult = new ValidationResult(msg, this, "Error", null, null);

                        validation.AddResult(valresult);
                    }
                }
                catch (DataException e)
                {
                    _logger.Error(e);

                    var msg = "There was an issue processing the request, please try again";
                    var valresult = new ValidationResult(msg, this, "Error", null, null);

                    validation.AddResult(valresult);
                }
            }

            var results = new PostResponse { Id = id, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        public PostResponse Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _logger.Info(entity.ToJson());

            var id         = 0;
            var validation = new ValidationResults();

            try
            {
                if (_repository.Get(entity.Id) != null)
                {
                    id = _repository.Update(entity, properties);
                }
                else
                {
                    var msg = "Item could not be found.";
                    var valresult = new ValidationResult(msg, this, "Error", null, null);

                    validation.AddResult(valresult);
                }
            }
            catch (DataException e)
            {
                _logger.Error(e);

                var msg = "There was an issue processing the request, please try again";
                var valresult = new ValidationResult(msg, this, "Error", null, null);

                validation.AddResult(valresult);
            }

            var results = new PostResponse { Id = id, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        #endregion

        #region PRIVATE METHODS

        private ValidationResults ValidateEntity(TEntity entity)
        {
            var validator  = ValidationFactory.CreateValidator<TEntity>();
            var validation = validator.Validate(entity);

            return validation;
        }

        #endregion
    }
}

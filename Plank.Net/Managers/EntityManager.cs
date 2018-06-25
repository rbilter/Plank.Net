using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;
using Plank.Net.Utilities;
using System;
using System.Data;
using System.Linq.Expressions;

namespace Plank.Net.Managers
{
    public class EntityManager<T> : IManager<T> where T : Entity
    {
        #region MEMBERS

        protected IRepository<T> _repository;
        protected ILogger<T> _logger;

        #endregion

        #region CONSTRUCTORS

        public EntityManager(IRepository<T> repository, ILogger<T> logger)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
            _logger     = logger ?? throw new ArgumentNullException("logger");
        }

        #endregion

        #region METHODS

        public virtual PostResponse Create(T entity)
        {
            _logger.Info(entity.ToJson());

            var newguid    = Guid.Empty;
            var validation = ValidateEntity(entity);

            if (validation.IsValid)
            {
                try
                {
                    newguid = _repository.Create(entity);
                }
                catch (DataException e)
                {
                    _logger.Error(e);

                    var msg = "There was an issue processing the request, please try again";
                    var valresult = new ValidationResult(msg, this, "Error", null, null);

                    validation.AddResult(valresult);
                }
            }

            var results = new PostResponse { Id = newguid, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        public virtual PostResponse Delete(Guid id)
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

        public virtual GetResponse<T> Get(Guid id)
        {
            _logger.Info(id);
            GetResponse<T> result = null;

            try
            {
                var item = _repository.Get(id);
                result = new GetResponse<T>(item);
                result.IsValid = true;
            }
            catch (DataException e)
            {
                _logger.Error(e);
                result = new GetResponse<T>();
                result.IsValid = false;
                result.Message = "There was an issue processing the request, please try again";
            }

            _logger.Info(result.ToJson());
            return result;
        }

        public virtual PostEnumerationResponse<T> Search(T entity)
        {
            throw new NotImplementedException("Need to implement");
        }

        public virtual PostResponse Update(T entity)
        {
            _logger.Info(entity.ToJson());

            var guid      = Guid.Empty;
            var validation = ValidateEntity(entity);

            if (validation.IsValid)
            {
                try
                {
                    if (_repository.Get(entity.Id) != null)
                    {
                        guid = _repository.Update(entity);
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

            var results = new PostResponse { Id = guid, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        public virtual PostResponse Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            _logger.Info(entity.ToJson());

            var guid       = Guid.Empty;
            var validation = new ValidationResults();

            try
            {
                if (_repository.Get(entity.Id) != null)
                {
                    guid = _repository.Update(entity, properties);
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

            var results = new PostResponse { Id = guid, ValidationResults = validation };
            _logger.Info(results.ToJson());

            return results;
        }

        #endregion

        #region PRIVATE METHODS

        private ValidationResults ValidateEntity(T entity)
        {
            var validator  = ValidationFactory.CreateValidator<T>();
            var validation = validator.Validate(entity);

            return validation;
        }

        #endregion
    }
}

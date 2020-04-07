using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Contracts;
using Plank.Net.Data;
using Plank.Net.Models;
using Plank.Net.Profiles;
using Plank.Net.Utilities;
using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public sealed class EntityManager<TEntity> : IManager<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly IRepository<TEntity> _repository;
        private readonly ILogger _logger;

        #endregion

        #region CONSTRUCTORS

        public EntityManager(DbContext context)
            : this(new EntityRepository<TEntity>(context), new Logger<TEntity>())
        {

        }

        public EntityManager(IRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger     = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region METHODS

        public async Task<PlankPostResponse<TEntity>> AddAsync(TEntity item)
        {
            _logger.Info(item.ToJson());

            var validation = ValidateEntity(item);

            if (validation.IsValid)
            {
                try
                {
                    await _repository.AddAsync(item);
                }
                catch (DataException e)
                {
                    _logger.Error(e);

                    var msg = "There was an issue processing the request, please try again";
                    var valresult = new ValidationResult(msg, this, "Error", null, null);

                    validation.AddResult(valresult);
                }
            }

            var results = new PlankPostResponse<TEntity> { Item = item, ValidationResults = Mapping<TEntity>.Mapper.Map<PlankValidationResults>(validation) };
            _logger.Info(results.ToJson());

            return results;
        }

        public async Task<PlankDeleteResponse> DeleteAsync(int id)
        {
            _logger.Info(id);

            var validation = new ValidationResults();

            try
            {
                if (await _repository.GetAsync(id) != null)
                {
                    await _repository.DeleteAsync(id);
                }
            }
            catch (DataException e)
            {
                _logger.Error(e);

                var msg = "There was an issue processing the request, please try again";
                var valresult = new ValidationResult(msg, this, "Error", null, null);

                validation.AddResult(valresult);
            }

            var results = new PlankDeleteResponse { Id = id, ValidationResults = Mapping<TEntity>.Mapper.Map<PlankValidationResults>(validation) };

            _logger.Info(results.ToJson());
            return results;
        }

        public async Task<PlankGetResponse<TEntity>> GetAsync(int id)
        {
            _logger.Info(id);
            PlankGetResponse<TEntity> result;

            try
            {
                var item = await _repository.GetAsync(id);
                result = new PlankGetResponse<TEntity>(item)
                {
                    IsValid = true
                };
            }
            catch (DataException e)
            {
                _logger.Error(e);
                result = new PlankGetResponse<TEntity>()
                {
                    IsValid = false,
                    Message = "There was an issue processing the request, please try again"
                };
            }

            _logger.Info(result.ToJson());
            return result;
        }

        public async Task<PlankEnumerableResponse<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize)
        {
            expression = expression ?? (f => true);

            var serializer = new ExpressionSerializer(new JsonSerializer());
            _logger.Info(serializer.SerializeText(expression));

            PlankEnumerableResponse<TEntity> result = null;
            try
            {
                var pagedList  = await _repository.SearchAsync(expression, pageNumber, pageSize);
                result         = Mapping<TEntity>.Mapper.Map<PlankEnumerableResponse<TEntity>>(pagedList);
                result.IsValid = true;
            }
            catch(DataException e)
            {
                _logger.Error(e);
                result = new PlankEnumerableResponse<TEntity>()
                {
                    IsValid = false,
                    Message = "There was an issue processing the request, please try again"
                };
            }

            _logger.Info(result.ToJson());
            return result;
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity item)
        {
            _logger.Info(item.ToJson());

            TEntity existing = null;
            var validation = ValidateEntity(item);

            if (validation.IsValid)
            {
                try
                {
                    existing = await _repository.GetAsync(item.Id);
                    if (existing != null)
                    {
                        foreach (var p in GetProperties(item))
                        {
                            p.SetValue(existing, p.GetValue(item));
                        };

                        await _repository.UpdateAsync(existing);
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

            var results = new PlankPostResponse<TEntity> { Item = existing, ValidationResults = Mapping<TEntity>.Mapper.Map<PlankValidationResults>(validation) };
            _logger.Info(results.ToJson());

            return results;
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity item, params Expression<Func<TEntity, object>>[] properties)
        {
            _logger.Info(item.ToJson());

            TEntity existing = null;
            var validation = new ValidationResults();

            try
            {
                existing = item != null ? await _repository.GetAsync(item.Id) : null;
                if (existing != null)
                {
                    // Assign values from item to the existing entity
                    foreach (var p in properties)
                    {
                        var operand = p.Body as MemberExpression ?? (p.Body as UnaryExpression).Operand as MemberExpression;
                        existing.GetType().GetProperty(operand.Member.Name).SetValue(existing, item.GetType().GetProperty(operand.Member.Name).GetValue(item));
                    }

                    // Assign values from existing back to item for validation
                    foreach (var p in GetProperties(item))
                    {
                        item.GetType().GetProperty(p.Name).SetValue(item, existing.GetType().GetProperty(p.Name).GetValue(existing));
                    }

                    validation = ValidateEntity(item);
                    if(validation.IsValid)
                    {
                        await _repository.UpdateAsync(existing);
                    }
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

            var results = new PlankPostResponse<TEntity> { Item = existing, ValidationResults = Mapping<TEntity>.Mapper.Map<PlankValidationResults>(validation) };
            _logger.Info(results.ToJson());

            return results;
        }

        #endregion

        #region PRIVATE METHODS

        private List<PropertyInfo> GetProperties(TEntity item)
        {
            return item.GetType()
                       .GetProperties()
                       .Where(p => p.DeclaringType == item.GetType() && !p.IsDefined(typeof(InversePropertyAttribute), false))
                       .ToList();
        }

        private ValidationResults ValidateEntity(TEntity item)
        {
            var validator  = ValidationFactory.CreateValidator<TEntity>();
            var validation = validator.Validate(item);

            return validation;
        }

        #endregion
    }
}

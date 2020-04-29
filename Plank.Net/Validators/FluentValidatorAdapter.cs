using FluentValidation;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Models;
using System.Diagnostics.CodeAnalysis;

namespace Plank.Net.Validators
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "The class is created through late-bound reflection method")]
    internal sealed class FluentValidatorAdapter<TEntity> : PlankValidator<TEntity> where TEntity : class, IEntity
    {
        #region MEMBERS

        private readonly AbstractValidator<TEntity> _fluentValidator;

        #endregion

        #region CONSTRUCTORS

        public FluentValidatorAdapter(AbstractValidator<TEntity> fluentValidator)
        {
            _fluentValidator = fluentValidator;
        }

        #endregion

        #region METHODS

        public override ValidationResults Validate(TEntity item)
        {
            var validationResults = new ValidationResults();
            
            var results = _fluentValidator.Validate(item);
            foreach(var error in results.Errors)
            {
                validationResults.AddResult(new ValidationResult(error.ErrorMessage, null, error.PropertyName, error.ErrorCode, null));
            }

            return validationResults;
        }

        #endregion
    }
}

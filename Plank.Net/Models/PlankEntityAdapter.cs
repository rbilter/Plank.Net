using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;

namespace Plank.Net.Models
{
    [HasSelfValidation]
    public abstract class PlankEntityAdapter : IEntity, IPopulateTimeStamps
    {
        #region PROPERTIES

        public virtual int Id { get; set; }

        public virtual Guid GlobalId { get; set; }

        public virtual DateTime DateCreated { get; set; }

        public virtual DateTime DateLastModified { get; set; }

        #endregion

        #region METHODS

        public void PopulateTimeStamps()
        {
            EntityHelper.PopulateTimeStamps(this);
        }

        [SelfValidation]
        public void Validate(ValidationResults results)
        {
            EntityHelper.Validate(this, results);
        }

        #endregion
    }
}

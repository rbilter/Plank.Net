using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;

namespace Plank.Net.Models
{
    public interface IEntity
    {
        #region PROPERTIES

        int Id { get; set; }

        Guid GlobalId { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateLastModified { get; set; }

        #endregion

        #region METHODS

        void Validate(ValidationResults results);

        #endregion
    }
}

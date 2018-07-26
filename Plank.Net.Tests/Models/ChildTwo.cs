using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Data;

namespace Plank.Net.Tests.Models
{
    [HasSelfValidation]
    public class ChildTwo : Entity
    {
        #region PROPERTIES

        public Guid ParentEntityId { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES

        [ForeignKey("ParentEntityId")]
        public virtual ParentEntity ParentEntity { get; set; }

        #endregion
    }
}

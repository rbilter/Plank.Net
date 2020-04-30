using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Tests.TestHelpers
{
    [HasSelfValidation]
    public class ChildTwo : PlankEntity
    {
        #region PROPERTIES

        public int ParentEntityId { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES

        [ForeignKey("ParentEntityId")]
        public virtual ParentEntity ParentEntity { get; set; }

        #endregion
    }
}

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Tests.TestHelpers
{
    [HasSelfValidation]
    public class ChildOne : PlankEntity
    {
        #region PROPERTIES

        [Required]
        [MaxLength(50, ErrorMessage = "Address cannot be longer than 50 characters")]
        public string Address { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "City cannot be longer than 30 characters")]
        public string City { get; set; }

        public int ParentEntityId { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES


        [ForeignKey("ParentEntityId")]
        public virtual ParentEntity ParentEntity { get; set; }

        #endregion
    }
}

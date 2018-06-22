using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Tests
{
    [NotNullValidator(MessageTemplate = "ParentEntity cannot be null.")]
    internal class ParentEntity : Entity
    {
        #region PROPERTIES

        [Required(ErrorMessage = "First name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(30, ErrorMessage = "First name cannot be greater than 30 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(50, ErrorMessage = "Last name cannot be greater than 50 characters.")]
        public string LastName { get; set; }

        [NotMapped]
        public override Guid ParentId { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES

        [InverseProperty("ParentEntity")]
        public virtual IEnumerable<ChildEntity> ChildEntities { get; set; }

        #endregion
    }
}

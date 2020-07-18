using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Tests.TestHelpers
{
    [HasSelfValidation]
    [NotNullValidator(MessageTemplate = "ParentEntity cannot be null.")]
    public class ParentEntity : PlankEntity
    {
        #region PROPERTIES

        [Required(ErrorMessage = "First name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(30, ErrorMessage = "First name cannot be greater than 30 characters.")]
        public string FirstName { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Last name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(50, ErrorMessage = "Last name cannot be greater than 50 characters.")]
        public string LastName { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES

        [InverseProperty("ParentEntity")]
        public virtual ICollection<ChildOne> ChildOne { get; set; }

        [InverseProperty("ParentEntity")]
        public virtual ICollection<ChildTwo> ChildTwo { get; set; }

        #endregion
    }
}

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Tests.Models
{
    [HasSelfValidation]
    internal class ChildOne : ChildEntity
    {
        #region PROPERTIES

        [Required]
        [MaxLength(50, ErrorMessage = "Address cannot be longer than 50 characters")]
        public string Address { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "City cannot be longer than 30 characters")]
        public string City { get; set; }

        [Column("ParentEntityId")]
        public override Guid ParentId { get; set; }

        #endregion

        #region NAVIGATION PROPERTIES


        [ForeignKey("ParentId")]
        public virtual ParentEntity ParentEntity { get; set; }

        #endregion
    }
}

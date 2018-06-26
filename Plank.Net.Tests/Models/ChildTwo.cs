using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Net.Data;

namespace Plank.Net.Tests.Models
{
    [HasSelfValidation]
    public class ChildTwo : ChildEntity
    {
        #region NAVIGATION PROPERTIES


        [Column("ParentEntityId")]
        public override Guid ParentId { get; set; }

        #endregion
    }
}

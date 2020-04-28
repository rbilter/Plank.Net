using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plank.Net.Models
{
    [HasSelfValidation]
    public abstract class PlankEntity : IEntity, IPopulateTimeStamps
    {
        #region PROPERTIES

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid GlobalId { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateLastModified { get; set; }

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

using System;

namespace Plank.Net.Data
{
    public abstract class ChildEntity : Entity
    {
        #region CONSTRUCTORS

        public ChildEntity()
            : base()
        {
        }

        public ChildEntity(Guid id, DateTime dateCreated, DateTime dateLastModified)
            : base(id, dateCreated, dateLastModified)
        {

        }

        #endregion

        #region PROPERTIES

        public abstract Guid ParentId { get; set; }

        #endregion
    }
}
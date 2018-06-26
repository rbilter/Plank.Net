using System;

namespace Plank.Net.Data
{
    public abstract class ChildEntity : Entity
    {
        #region PROPERTIES

        public abstract Guid ParentId { get; set; }

        #endregion
    }
}